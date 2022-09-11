<template>
  <v-container class='my-5' style="max-width: 32rem">
    <v-simple-table dense>
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Order recieved, here is your reciept</v-toolbar-title>
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
  name: "Reciept",
  props: {
    origin: String,
    destination: String,
    company: String,
    departureDate: String,
    returnDate: String,
    zipCode: String,
    firstName: String,
    lastName: String,
    email: String,
    phone: Number,
    street: String,
    customers: [
      {
        firstName: String,
        lastName: String,
      },
    ],
    numberOfVehicles: Number,
    isRoundTrip: Boolean,
    totalPrice: Number,
  },
};
</script>