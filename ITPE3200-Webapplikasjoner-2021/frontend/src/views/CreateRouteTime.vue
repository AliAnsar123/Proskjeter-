<template>
  <v-container v-if="!isLoading" class='my-5' style="max-width: 32rem">
    <v-form v-model='isFormValid' ref='form'>
      <v-simple-table dense>
        <template v-slot:top>
          <v-toolbar flat>
            <v-toolbar-title>Create new route time</v-toolbar-title>
          </v-toolbar>
        </template>
        <tbody>
          <tr>
            <th>Property</th>
            <th>Value</th>
          </tr>
          <tr>
            <td>Route</td>
            <td>
              <v-select v-model="routetime.route.id" :items="routes" :item-text="itemText" item-value="id" :rules="[ruleRequired]"></v-select>
            </td>
          </tr>
          <tr>
            <td>Price</td>
            <td>
              <v-text-field v-model="routetime.price" type="number" :rules="[ruleRequired, rulePrice]"></v-text-field>
            </td>
          </tr>
          <tr>
            <td>Direction</td>
            <td>
              <v-switch v-model="routetime.direction"></v-switch>
            </td>
          </tr>
          <tr>
            <td>Date</td>
            <td>
              <v-text-field v-model="routetime.date" :rules="[ruleRequired, ruleDate]"></v-text-field>
            </td>
          </tr>
        </tbody>
      </v-simple-table>
    </v-form>
    <v-row class="pa-5" justify="space-between">
      <v-btn class="error" @click="cancel">Cancel</v-btn>
      <v-btn class="success" @click="save" :disabled="!isFormValid">Save</v-btn>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "CreateRouteTime",
  data() {
    return {
      routetime: {
        route: {
          id: null,
        },
        price: 0,
        date: "2021-11-25",
        direction: 0,
      },
      routes: Array,
      isFormValid: Boolean,
      isLoading: true,
      ruleRequired: v => !!v || "Required",
      ruleDate: v =>
        /^\d{4}-\d{2}-\d{2}$/.test(v) || "Date must be of format YYYY-MM-DD",
      rulePrice: v =>
        /^\d{1,5}$/.test(v) || "Pric must be between 1 and 5 digits",
    };
  },
  watch: {
    routetime: {
      handler() {
        this.$refs.form.validate();
      },
      deep: true,
    },
  },
  computed: {
    itemText() {
      return this.routetime.direction == 0 ? "name" : "nameReverse";
    },
  },
  methods: {
    cancel: function () {
      this.$router.push("/admin");
    },
    save: async function () {
      this.$refs.form.validate();
      console.log(this.routetime.direction);
      if (this.isFormValid) {
        const response = await fetch("/api/routetimes", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            route: {
              id: this.routetime.route.id,
            },
            date: this.routetime.date,
            price: parseInt(this.routetime.price),
            direction: this.routetime.direction ? 0 : 1,
          }),
        });
        if (response.ok) {
          this.$router.push("/admin");
        } else {
          const responseText = await response.text();
          alert(responseText);
          if (responseText == "Unauthorized") {
            this.$router.push("/login");
          }
        }
      }
    },
  },
  async mounted() {
    const response = await fetch("/api/routes");
    const routes = await response.json();
    this.routes = routes.map(route => ({
      ...route,
      name: route.origin.name + " - " + route.destination.name,
      nameReverse: route.destination.name + " - " + route.origin.name,
    }));
    this.isLoading = false;
  },
};
</script>