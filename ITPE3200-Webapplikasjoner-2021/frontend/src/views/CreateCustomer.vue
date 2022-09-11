<template>
  <v-container v-if="!isLoading" class='my-5' style="max-width: 32rem">
    <v-form v-model='isFormValid' ref='form'>
      <v-simple-table dense>
        <template v-slot:top>
          <v-toolbar flat>
            <v-toolbar-title>Create new customer</v-toolbar-title>
          </v-toolbar>
        </template>
        <tbody>
          <tr>
            <th>Property</th>
            <th>Value</th>
          </tr>
          <tr>
            <td>First name</td>
            <td>
              <v-text-field v-model="customer.firstName" :rules="[ruleRequired, ruleName]"></v-text-field>
            </td>
          </tr>
          <tr>
            <td>Last name</td>
            <td>
              <v-text-field v-model="customer.lastName" :rules="[ruleRequired, ruleName]"></v-text-field>
            </td>
          </tr>
          <tr>
            <td>Email</td>
            <td>
              <v-text-field v-model="customer.email" :rules="[ruleRequired, ruleEmail]"></v-text-field>
            </td>
          </tr>
          <tr>
            <td>Phone</td>
            <td>
              <v-text-field v-model="customer.phone" :rules="[ruleRequired, rulePhone]"></v-text-field>
            </td>
          </tr>
          <tr>
            <td>Street</td>
            <td>
              <v-text-field v-model="customer.street" :rules="[ruleRequired]"></v-text-field>
            </td>
          </tr>
          <tr>
            <td>Zip code</td>
            <td>
              <v-text-field v-model="customer.zipCode.id" :rules='[ruleRequired, ...rulesZipCode]' type='number' min='0001' max='9999' />
            </td>
          </tr>
          <tr>
            <td>City</td>
            <td>
              {{ customer.zipCode.city }}
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
  name: "CreateCustomer",
  data() {
    return {
      customer: Object,
      zipCodes: {},
      isFormValid: Boolean,
      isLoading: true,
      ruleRequired: v => !!v || "Required",
      ruleName: v =>
        /^[a-zA-ZæøåÆØÅ .'-]{2,20}$/.test(v) ||
        "Name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters",
      ruleEmail: v => /.+@.+\..+/.test(v) || "Invalid email",
      rulePhone: v => /^[0-9]{8}$/.test(v) || "Phone must consist of 8 numbers",
    };
  },
  watch: {
    "customer.zipCode.id": function () {
      this.customer.zipCode.city = this.zipCodes[this.customer.zipCode.id];
    },
  },
  computed: {
    rulesZipCode() {
      return [
        v => /^\d{4}$/.test(v) || "Ugyldig postkode",
        () => !!this.customer.zipCode.city || "Ukjent postkode",
      ];
    },
  },
  methods: {
    cancel: function () {
      this.$router.push("/admin");
    },
    save: async function () {
      this.$refs.form.validate();

      if (this.isFormValid) {
        const response = await fetch("/api/customers", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(this.customer),
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
  mounted() {
    fetch("/api/zipcodes")
      .then(response => response.json())
      .then(zipCodes => {
        zipCodes.forEach(zipCode => (this.zipCodes[zipCode.id] = zipCode.city));
        this.isLoading = false;
      });
  },
};
</script>