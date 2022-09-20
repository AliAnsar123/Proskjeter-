<template>
  <v-data-table :headers='headers' :items='zipcodes' @click:row="openZipCode" class="data-table" :loading="loading" loading-text="Loading...">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Zip codes</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark class="mb-2">New zip code </v-btn>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
export default {
  name: "ZipCodesTable",
  data() {
    return {
      loading: true,
      zipcodes: [],
      headers: [
        {
          text: "Zip code",
          value: "id",
        },
        {
          text: "City",
          value: "city",
        },
      ],
    };
  },
  methods: {
    openZipCode: function ({ id }) {
      this.$router.push(`/admin/zipcodes/${id}`);
    },
  },
  async mounted() {
    const response = await fetch("/api/zipcodes");
    this.zipcodes = await response.json();
    this.loading = false;
  },
};
</script>

<style scoped>
.data-table >>> tbody tr :hover {
  cursor: pointer;
}
</style>