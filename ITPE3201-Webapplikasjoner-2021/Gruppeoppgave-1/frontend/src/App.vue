<template>
  <v-app>
    <v-app-bar app>
      <v-container class='py-0 fill-height'>
        <v-btn to="/">Bestill båttur</v-btn>
        <v-spacer></v-spacer>
        <v-btn to="/admin">Admin</v-btn>
      </v-container>
    </v-app-bar>
    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>
