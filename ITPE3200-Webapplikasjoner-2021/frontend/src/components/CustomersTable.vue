<template>
  <v-data-table :headers='headers' :items='customers' @click:row="openCustomer" class="data-table" :loading="loading" loading-text="Loading...">
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Customers</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn color="primary" dark class="mb-2" @click="newCustomer">New customer </v-btn>
      </v-toolbar>
    </template>
  </v-data-table>
</template>

<script>
export default {
  name: "CustomersTable",
  data() {
    return {
      loading: true,
      customers: [],
      headers: [
        {
          text: "Customer ID",
          value: "id",
        },
        {
          text: "First name",
          value: "firstName",
        },
        {
          text: "Last name",
          value: "lastName",
        },
        {
          text: "Email",
          value: "email",
        },
        {
          text: "Phone",
          value: "phone",
        },
        {
          text: "Zip code",
          value: "zipCode.id",
        },
        {
          text: "City",
          value: "zipCode.city",
        },
        {
          text: "Street",
          value: "street",
        },
      ],
    };
  },
  methods: {
    newCustomer: function () {
      this.$router.push(`/admin/customers/new`);
    },
    openCustomer: function ({ id }) {
      this.$router.push(`/admin/customers/${id}`);
    },
  },
  async mounted() {
    const response = await fetch("/api/customers");
    if (!response.ok) {
      this.$router.push("/login");
    }
    this.customers = await response.json();
    this.loading = false;
  },
};
</script>

<style scoped>
.data-table >>> tbody tr :hover {
  cursor: pointer;
}
</style>