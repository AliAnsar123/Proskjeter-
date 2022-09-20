<template>
  <v-data-table :headers='headers' :items='routes' @click:row="openRoute" class="data-table" :loading="loading" loading-text="Loading...">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Routes</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark class="mb-2" @click="newRoute">New route </v-btn>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
export default {
  name: "RoutesTable",
  data() {
    return {
      loading: true,
      routes: [],
      headers: [
        {
          text: "Route ID",
          value: "id",
        },
        {
          text: "Origin port",
          value: "origin.name",
        },
        {
          text: "Destination port",
          value: "destination.name",
        },
        {
          text: "Company",
          value: "company.name",
        },
      ],
    };
  },
  methods: {
    newRoute: function () {
      this.$router.push(`/admin/routes/new`);
    },
    openRoute: function ({ id }) {
      this.$router.push(`/admin/routes/${id}`);
    },
  },
  async mounted() {
    const response = await fetch("/api/routes");
    this.routes = await response.json();
    this.loading = false;
  },
};
</script>

<style scoped>
.data-table >>> tbody tr :hover {
  cursor: pointer;
}
</style>