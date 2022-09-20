<template>
  <v-row>
    <v-col>
      <departure-selector :departure-date='departureDate' :departure-dates='departureDates' :return-date='returnDate' :rule-required='ruleRequired' @update='$emit("update:departureDate", $event)'>
        >
      </departure-selector>
    </v-col>

    <v-col v-if='isRoundTrip'>
      <return-selector :departure-date='departureDate' :return-date='returnDate' :return-dates='returnDates' :rule-required='ruleRequired' @update='$emit("update:returnDate", $event)'>
        >
      </return-selector>
    </v-col>
  </v-row>
</template>

<script>
import DepartureSelector from "@/components/DepartureSelector.vue";
import ReturnSelector from "@/components/ReturnSelector.vue";

export default {
  name: "RouteTimesSelector",
  props: {
    isRoundTrip: Boolean,
    route: Object,
    ruleRequired: Function,
    departureDate: String,
    returnDate: String,
  },
  data() {
    return {
      departureDates: [],
      returnDates: [],
    };
  },
  components: {
    DepartureSelector,
    ReturnSelector,
  },
  watch: {
    // når rute endres nuller vi datofeltene og finner nye avgangs- og returdatoer/-tider
    route: async function () {
      this.$emit("update:departureDate", null);
      this.$emit("update:returnDate", null);

      if (this.route) {
        try {
          const response = await fetch(`/api/routetimes/${this.route.value}`);
          const routeTimes = await response.json();
          this.$emit("update:routeTimes", routeTimes);
          this.departureDates = routeTimes
            .filter(rt => rt.direction == this.route.direction)
            .map(rt => new Date(rt.date).setHours(0, 0, 0));
          this.returnDates = routeTimes
            .filter(rt => rt.direction == (this.route.direction == 0 ? 1 : 0))
            .map(rt => new Date(rt.date).setHours(0, 0, 0));
        } catch {
          alert("Noe gikk galt, prøv igjen senere");
        }
      }
    },
  },
};
</script>