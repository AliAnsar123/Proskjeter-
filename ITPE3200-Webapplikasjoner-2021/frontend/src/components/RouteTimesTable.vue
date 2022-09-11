<template>
  <v-data-table :headers='headers' :items='routeTimes' class="data-table" :loading="loading" loading-text="Loading...">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Route times</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark class="mb-2" @click="newRouteTime">New route time </v-btn>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
export default {
  name: "RouteTimesTable",
  data() {
    return {
      loading: true,
      routeTimes: [],
      headers: [
        {
          text: "Route time ID",
          value: "id",
        },
        {
          text: "Date",
          value: "dateText",
        },
        {
          text: "Price",
          value: "price",
        },
        {
          text: "Route",
          value: "routeText",
        },
        {
          text: "Company",
          value: "route.company.name",
        },
      ],
    };
  },
  methods: {
    newRouteTime: function () {
      this.$router.push(`/admin/routeTimes/new`);
    },
  },
  mounted() {
    fetch("/api/routeTimes")
      .then(response => response.json())
      .then(routeTimes => {
        this.routeTimes = routeTimes.map(rt => ({
          ...rt,
          dateText: new Date(rt.date).toLocaleDateString(),
          routeText: `${rt.route.origin.name} - ${rt.route.destination.name}`,
        }));
      });

    this.loading = false;
  },
};
</script>

<style scoped>
.data-table >>> tbody tr :hover {
  cursor: pointer;
}
</style>