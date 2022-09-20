<template>
  <v-container v-if="!isLoading" class='my-5' style="max-width: 32rem">
    <v-form v-model='isFormValid' ref='form'>
      <v-simple-table dense>
        <template v-slot:top>
          <v-toolbar flat>
            <v-toolbar-title>Edit company</v-toolbar-title>
          </v-toolbar>
        </template>
        <tbody>
          <tr>
            <th>Property</th>
            <th>Value</th>
          </tr>
          <tr>
            <td>Id</td>
            <td>{{ company.id }}</td>
          </tr>
          <tr>
            <td>Name</td>
            <td>
              <v-text-field v-model="company.name" :rules="[ruleRequired, ruleName]"></v-text-field>
            </td>
          </tr>
        </tbody>
      </v-simple-table>
    </v-form>
    <v-row class="pa-5" justify="space-between">
      <v-btn @click="cancel">Cancel</v-btn>
      <v-btn class="error" @click="deleteCompany">Delete</v-btn>
      <v-btn class="success" @click="save" :disabled="!isFormValid">Save</v-btn>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "EditCompany",
  data() {
    return {
      company: Object,
      isFormValid: Boolean,
      ruleRequired: v => !!v || "Required",
      ruleName: v =>
        /^[a-zA-ZæøåÆØÅ .'-]{2,20}$/.test(v) ||
        "Name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters",
    };
  },
  methods: {
    cancel: function () {
      this.$router.push("/admin");
    },
    deleteCompany: async function () {
      const response = await fetch(`/api/companies/${this.$route.params.id}`, {
        method: "DELETE",
      });
      if (response.ok) {
        this.$router.push("/admin");
      } else {
        alert(await response.text());
        if (response.text == "Unauthorized") {
          this.$router.push("/login");
        }
      }
    },
    save: async function () {
      this.$refs.form.validate();

      if (this.isFormValid) {
        const response = await fetch(
          `/api/companies/${this.$route.params.id}`,
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(this.company),
          }
        );
        if (response.ok) {
          this.$router.push("/admin");
        } else {
          alert(await response.text());
          if (response.text == "Unauthorized") {
            this.$router.push("/login");
          }
        }
      }
    },
  },
  mounted() {
    fetch(`/api/companies/${this.$route.params.id}`)
      .then(response => {
        if (!response.ok) {
          throw new Error();
        }
        return response.json();
      })
      .then(company => {
        this.company = company;
        this.isLoading = false;
      })
      .catch(() => this.$router.push("/login"));
  },
};
</script>