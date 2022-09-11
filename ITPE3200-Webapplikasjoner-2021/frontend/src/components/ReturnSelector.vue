<template>
  <v-menu v-model='returnDateMenu' :nudge-right='40' transition='scale-transition' offset-y min-width='auto'>
    <template v-slot:activator='{ on, attrs }'>
      <v-text-field :value='returnDate' label='Retur' prepend-inner-icon='mdi-calendar' readonly v-bind='attrs' v-on='on' required :rules='[ruleRequired]' />
    </template>
    <v-date-picker :value='returnDate' @input='returnDateMenu = false, $emit("update", $event)' :allowed-dates='allowedReturnDates' />
  </v-menu>
</template>

<script>
export default {
  name: "ReturnSelector",
  props: {
    departureDate: String,
    returnDate: String,
    returnDates: Array,
    ruleRequired: Function,
  },
  data() {
    return {
      returnDateMenu: false,
    };
  },
  methods: {
    allowedReturnDates: function (date) {
      // filter sjekker at returdatoen er etter den valgte avgangsdatoen
      return this.returnDates
        ?.filter(returnDate => {
          return (
            (this.departureDate == null ||
              returnDate > new Date(this.departureDate).setHours(0, 0, 0)) &&
            returnDate > new Date().getTime()
          );
        })
        .includes(new Date(date).setHours(0, 0, 0));
    },
  },
};
</script>