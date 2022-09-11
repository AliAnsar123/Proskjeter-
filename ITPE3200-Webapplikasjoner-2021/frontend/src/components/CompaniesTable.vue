<template>
  <v-data-table :headers='headers' :items='companies' @click:row="openCompany" class="data-table" :loading="loading" loading-text="Loading...">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Companies</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark class="mb-2" @click="newCompany">New company </v-btn>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
export default {
  name: "CompaniesTable",
  data() {
    return {
      loading: true,
      companies: [],
      headers: [
        {
          text: "Company ID",
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
    newCompany: function () {
      this.$router.push("/admin/companies/new");
    },
    openCompany: function ({ id }) {
      this.$router.push(`/admin/companies/${id}`);
    },
  },
  async mounted() {
    const response = await fetch("/api/companies");
    this.companies = await response.json();
    this.loading = false;
  },
};
</script>

<style scoped>
.data-table >>> tbody tr :hover {
  cursor: pointer;
}
</style>