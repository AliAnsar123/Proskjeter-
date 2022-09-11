<template>
  <v-container v-if="!isLoading" class='my-5' style="max-width: 32rem">
    <v-form v-model='isFormValid' ref='form'>
      <v-simple-table dense>
        <template v-slot:top>
          <v-toolbar flat>
            <v-toolbar-title>Create new route</v-toolbar-title>
          </v-toolbar>
        </template>
        <tbody>
          <tr>
            <th>Property</th>
            <th>Value</th>
          </tr>
          <tr>
            <td>Origin port</td>
            <td>
              <v-select v-model="route.origin.id" :items="ports" item-text="name" item-value="id" :rules="[ruleRequired, ruleUnique]"></v-select>
            </td>
          </tr>
          <tr>
            <td>Destination port</td>
            <td>
              <v-select v-model="route.destination.id" :items="ports" item-text="name" item-value="id" :rules="[ruleRequired, ruleUnique]"></v-select>
            </td>
          </tr>
          <tr>
            <td>Company</td>
            <td>
              <v-select v-model="route.company.id" :items="companies" item-text="name" item-value="id" :rules="[ruleRequired]"></v-select>
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
  name: "CreateRoute",
  data() {
    return {
      route: {
        origin: {
          id: null,
        },
        destination: {
          id: null,
        },
        company: {
          id: null,
        },
      },
      companies: Array,
      ports: Array,
      isFormValid: Boolean,
      isLoading: true,
      ruleRequired: v => !!v || "Required",
    };
  },
  computed: {
    ruleUnique() {
      return () =>
        this.route.origin.id != this.route.destination.id ||
        "Origin and destination cannot be the same";
    },
  },
  watch: {
    route: {
      handler() {
        this.$refs.form.validate();
      },
      deep: true,
    },
  },
  methods: {
    cancel: function () {
      this.$router.push("/admin");
    },
    save: async function () {
      this.$refs.form.validate();

      if (this.isFormValid) {
        const response = await fetch("/api/routes", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(this.route),
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
    const portsResponse = await fetch("/api/ports");
    this.ports = await portsResponse.json();

    const companiesResponse = await fetch("/api/companies");
    this.companies = await companiesResponse.json();

    this.isLoading = false;
  },
};
</script>