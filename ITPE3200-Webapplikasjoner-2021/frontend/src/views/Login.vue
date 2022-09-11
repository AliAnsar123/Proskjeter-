<template>
  <v-container class="my-5" style="max-width: 16rem">
    <v-form v-model="valid" ref="form">
      <v-row>
        <v-col cols="12">
          <h1>Admin login</h1>
          <v-text-field v-model="username" :rules="usernameRules" label="User name" required></v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field v-model="password" :rules="passwordRules" label="Password" required type="password"></v-text-field>
        </v-col>
        <v-col cols="12">
          <p class="red--text">{{ errortext }}</p>
          <v-btn color="primary" @click="login">Login</v-btn>
        </v-col>
      </v-row>
    </v-form>
  </v-container>
</template>

<script>
export default {
  name: "Login",
  data() {
    return {
      valid: null,
      username: null,
      password: null,
      errortext: null,
      usernameRules: [
        v => !!v || "Username is required",
        v =>
          /^[a-zA-ZæøåÆØÅ.-]{2,20}$/.test(v) ||
          "Username must contain between 2 and 20 characters",
      ],
      passwordRules: [
        v => !!v || "Password is required",
        v =>
          /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/.test(v) ||
          "Password must contain at least 6 characters, containing at least one letter and one number",
      ],
    };
  },
  methods: {
    async login() {
      this.$refs.form.validate();

      try {
        if (this.valid) {
          const response = await fetch("/api/login", {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({
              Username: this.username,
              Password: this.password,
            }),
          });

          if (!response.ok) {
            throw new Error();
          } else {
            this.$router.push("/admin");
          }
        }
      } catch {
        this.errortext = "Wrong username or password";
      }
    },
  },
};
</script>