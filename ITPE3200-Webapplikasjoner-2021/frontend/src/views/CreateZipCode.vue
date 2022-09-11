<template>
  <v-container class='my-5' style="max-width: 32rem">
    <v-form v-model='isFormValid' ref='form'>
      <v-simple-table dense>
        <template v-slot:top>
          <v-toolbar flat>
            <v-toolbar-title>Create new zipCode</v-toolbar-title>
          </v-toolbar>
        </template>
        <tbody>
          <tr>
            <th>Property</th>
            <th>Value</th>
          </tr>
          <tr>
            <td>Name</td>
            <td>
              <v-text-field v-model="zipCode.name" :rules="[ruleRequired, ruleName]"></v-text-field>
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
name: "zipCode",
  data() {
    return {
        zipCode: {
        name: "",
      },
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
    save: async function () {
      this.$refs.form.validate();

      if (this.isFormValid) {
          const response = await fetch("/api/zipCodes", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
            body: JSON.stringify(this.zipCode),
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
};
</script>