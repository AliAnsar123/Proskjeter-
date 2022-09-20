<template>
  <v-container class='my-5'>
    <v-btn color="error" dark class="mb-2" @click="logout">Logout</v-btn>
    <orders-table></orders-table>
    <v-divider class="my-5"></v-divider>
    <companies-table></companies-table>
    <v-divider class="my-5"></v-divider>
    <customers-table></customers-table>
    <v-divider class="my-5"></v-divider>
    <ports-table></ports-table>
    <v-divider class="my-5"></v-divider>
    <routes-table></routes-table>
    <v-divider class="my-5"></v-divider>
    <route-times-table></route-times-table>
    <v-divider class="my-5"></v-divider>
    <zip-codes-table></zip-codes-table>
  </v-container>
</template>

<script>
import OrdersTable from "@/components/OrdersTable.vue";
import CompaniesTable from "@/components/CompaniesTable.vue";
import CustomersTable from "@/components/CustomersTable.vue";
import PortsTable from "@/components/PortsTable.vue";
import RoutesTable from "@/components/RoutesTable.vue";
import RouteTimesTable from "@/components/RouteTimesTable.vue";
import ZipCodesTable from "@/components/ZipCodesTable.vue";

export default {
  name: "Admin",
  components: {
    OrdersTable,
    CompaniesTable,
    CustomersTable,
    PortsTable,
    RoutesTable,
    RouteTimesTable,
    ZipCodesTable,
  },
  methods: {
    logout: function () {
      fetch("/api/login", { method: "DELETE" }).then(
        this.$router.push("/login")
      );
    },
  },
};
</script>
