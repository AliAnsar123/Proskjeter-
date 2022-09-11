<template>
  <v-menu v-model='departureDateMenu' :nudge-right='40' transition='scale-transition' offset-y min-width='auto'>
    <template v-slot:activator='{ on, attrs }'>
      <v-text-field :value='departureDate' label='Utreise' prepend-inner-icon='mdi-calendar' readonly v-bind='attrs' v-on='on' required :rules='[ruleRequired]' />
    </template>
    <v-date-picker :value='departureDate' @input='departureDateMenu = false, $emit("update", $event)' :allowed-dates='allowedDepartureDates' />
  </v-menu>
</template>

<script>
export default {
  name: "DepartureSelector",
  props: {
    departureDate: String,
    departureDates: Array,
    returnDate: String,
    ruleRequired: Function,
  },
  data() {
    return {
      departureDateMenu: false,
    };
  },
  methods: {
    allowedDepartureDates: function (date) {
      // filter sjekker at avgangsdatoen er før den valgte avgangsdatoen
      return this.departureDates
        ?.filter(departureDate => {
          return (
            (this.returnDate == null ||
              departureDate < new Date(this.returnDate).setHours(0, 0, 0)) &&
            departureDate > new Date().getTime()
          );
        })
        .includes(new Date(date).setHours(0, 0, 0));
    },
  },
};
</script>