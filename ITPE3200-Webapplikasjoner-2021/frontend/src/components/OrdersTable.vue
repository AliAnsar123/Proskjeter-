<template>
  <v-data-table :headers='headers' :items='orders' @click:row="openOrder" class="data-table">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Orders</v-toolbar-title>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
export default {
  name: "OrdersTable",
  data() {
    return {
      orders: [],
      headers: [
        {
          text: "ID",
          value: "id",
        },
        {
          text: "Customer",
          value: "mainCustomer.email",
        },
        {
          text: "Origin",
          value: "departureRouteTime.route.origin.name",
        },
        {
          text: "Departure",
          value: "departure",
        },
        {
          text: "Destination",
          value: "departureRouteTime.route.destination.name",
        },
        {
          text: "Return",
          value: "return",
        },
        {
          text: "Tur-Retur",
          value: "turRetur",
        },
        {
          text: "Passengers",
          value: "numberOfPassengers",
        },
        {
          text: "Vehicles",
          value: "numberOfVehicles",
        },
        {
          text: "Total price",
          value: "totalPrice",
        },
      ],
    };
  },
  methods: {
    // eslint-disable-next-line no-unused-labels
    openOrder: function ({ id }) {
      this.$router.push(`/admin/orders/${id}`);
    },
  },
  mounted() {
    fetch("/api/orders")
      .then(response => {
        if (!response.ok) {
          throw Error();
        }
        return response.json();
      })
      .then(orders => {
        this.orders = orders.map(order => {
          return {
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
        });
      })
      .catch(() => this.$router.push("/login"));
  },
};
</script>

<style scoped>
.data-table >>> tbody tr :hover {
  cursor: pointer;
}
</style>