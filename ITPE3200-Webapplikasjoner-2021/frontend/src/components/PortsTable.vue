<template>
  <v-data-table :headers='headers' :items='ports' @click:row="editPort" class="data-table" :loading="loading" loading-text="Loading...">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Ports</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark class="mb-2" @click="newPort">New port</v-btn>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
export default {
  name: "PortsTable",
  data() {
    return {
      loading: true,
      ports: [],
      headers: [
        {
          text: "Port ID",
          value: "id",
        },
        {
          text: "Name",
          value: "name",
        },
      ],
    };
  },
  methods: {
    newPort: function () {
      this.$router.push(`/admin/ports/new`);
    },
    editPort: function ({ id }) {
      this.$router.push(`/admin/ports/${id}`);
    },
  },
  async mounted() {
    const response = await fetch("/api/ports");
    this.ports = await response.json();
    this.loading = false;
  },
};
</script>

<style scoped>
.data-table >>> tbody tr :hover {
  cursor: pointer;
}
</style>