<template>
  <v-row>
    <v-col>
      <v-select v-model="originPortId" :items="routes" :rules="[ruleRequired]" label="Fra havn" required prepend-inner-icon="mdi-ferry" />
    </v-col>

    <v-col>
      <v-select :value="route" @input="$emit('update:route', $event)" :items="routeDestinations" :rules="[ruleRequired]" label="Til havn" required return-object no-data-text="Du må velge avgangshavn først." />
    </v-col>

    <v-col cols="5">
      <v-radio-group :value="isRoundTrip" @change="$emit('update:isRoundTrip', $event)" row>
        <v-radio label="Én vei" :value="false"> </v-radio>
        <v-radio label="Tur-retur" :value="true"> </v-radio>
      </v-radio-group>
    </v-col>
  </v-row>
</template>

<script>
export default {
  name: "RouteSelector",
  props: {
    isRoundTrip: Boolean,
    route: Object,
    ruleRequired: Function,
  },
  data() {
    return {
      originPortId: null,
      routes: [],
    };
  },
  computed: {
    routesForSelectedOriginPort() {
      return this.routes.filter(route => route.value == this.originPortId);
    },
    // retunerer array med gyldige "til-havner" for den valgte utreisehavnen
    routeDestinations() {
      return this.routesForSelectedOriginPort.map(route => ({
        direction: route.direction,
        value: route.id,
        text:
          route.direction == 0 ? route.destination?.name : route.origin?.name,
      }));
    },
  },
  watch: {
    // forhindrer at man kan bytte fra f.eks. oslo - kiel til kiel - oslo uten å få oppdatert datoer, da de to har samme rute-id
    originPortId: function () {
      this.$emit("update:route", null);
    },
  },
  mounted() {
    fetch("/api/routes")
      .then(response => response.json())
      .then(routes => {
        this.routes = routes.flatMap(route => [
          // returnerer to objekter her så man kan velge feks å dra Kiel til Oslo selv om ruten går fra Oslo til Kiel
          // valgte å bruke retning i stedet for å lage en egen rute for "returen"
          {
            direction: 0,
            text: route.origin.name,
            value: route.origin.id,
            ...route,
          },
          {
            direction: 1,
            text: route.destination.name,
            value: route.destination.id,
            ...route,
          },
        ]);
      })
      .catch(error => {
        alert("Noe gikk galt: " + error);
      });
  },
};
</script>
