<template>
  <v-container class='my-5' style="max-width: 36rem">
    <v-simple-table dense>
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Order {{ $route.params.id }}</v-toolbar-title>
        </v-toolbar>
      </template>
      <tbody>
        <tr>
          <td>Customer</td>
          <td>{{ firstName }} {{ lastName }}</td>
        </tr>
        <tr>
          <td>Email</td>
          <td>{{ email }}</td>
        </tr>
        <tr>
          <td>Phone</td>
          <td>{{ phone }}</td>
        </tr>
        <tr>
          <td>Street</td>
          <td>{{ street }}</td>
        </tr>
        <tr>
          <td>Zip code</td>
          <td>{{ zipCode }}</td>
        </tr>
        <template v-for="(passenger, index) in customers">
          <tr :key="index">
            <td>{{ `Passenger ${index + 1}` }}</td>
            <td>{{ passenger.firstName }} {{ passenger.lastName }}</td>
          </tr>
        </template>
        <tr>
          <td>Vehicles</td>
          <td>{{ numberOfVehicles }}</td>
        </tr>
        <tr>
          <td>Route</td>
          <td>{{ origin }} - {{ destination}}</td>
        </tr>
        <tr>
          <td>Departure date</td>
          <td>{{ new Date(departureDate).toLocaleDateString() }}</td>
        </tr>
        <tr v-if="isRoundTrip">
          <td>Return date</td>
          <td>{{ new Date(returnDate).toLocaleDateString() }}</td>
        </tr>
        <tr>
          <td>Company</td>
          <td>{{ company }}</td>
        </tr>
        <tr>
          <td>Total price</td>
          <td>kr {{ totalPrice }}</td>
        </tr>
      </tbody>
    </v-simple-table>
  </v-container>
</template>

<script>
export default {
  name: "OrderView",
  data() {
    return {
      order: null,
    };
  },
  mounted() {
    fetch(`/api/orders/${this.$route.params.id}`)
      .then(response => response.json())
      .then(order => {
        this.order = {
          departure: new Date(
            order.departureRouteTime.date
          ).toLocaleDateString(),
          return:
            order.returnRouteTime != null
              ? new Date(order.returnRouteTime.date).toLocaleDateString()
              : null,
          totalPrice:
            (
              (order.departureRouteTime.price +
                (order.returnRouteTime != null
                  ? order.returnRouteTime?.price
                  : 0)) *
              (order.customers.length + 1 + order.numberOfVehicles * 3)
            ).toLocaleString("nb-NO", {
              style: "currency",
              currency: "NOK",
              maximumFractionDigits: 0,
            }) + ",-",
          turRetur: order.isRoundTrip ? "✓" : "𐄂",
          numberOfPassengers: order.customers.length + 1,
          ...order,
        };
      })
      .catch(error => {
        alert("Noe gikk galt: " + error);
      });
  },
};
</script>