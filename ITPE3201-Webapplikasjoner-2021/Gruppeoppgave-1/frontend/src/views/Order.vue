<template>
  <v-container class='my-5'>
    <v-form v-model='isFormValid' ref='form'>
      <v-container>
        <h1>Hvor vil du reise?</h1>

        <route-selector :is-round-trip.sync="isRoundTrip" :route.sync="route" :rule-required="ruleRequired">
        </route-selector>

        <route-times-selector :departure-date.sync="departureDate" :return-date.sync="returnDate" :is-round-trip="isRoundTrip" :route="route" :route-times.sync="routeTimes" :rule-required="ruleRequired">
        </route-times-selector>

        <v-row>
          <v-col>
            <v-text-field v-model='numberOfPassengers' type='number' label='Passasjerer' min='1' max='9' :prepend-inner-icon='(numberOfPassengers > 1) ? "mdi-account-multiple" : "mdi-account"' required :rules='[ruleRequired, rulePassengers]' />
          </v-col>
          <v-col>
            <v-text-field v-model='numberOfVehicles' type='number' label='Kjøretøy' min='0' max='9' :prepend-inner-icon='(numberOfVehicles > 1) ? "mdi-car-multiple" : "mdi-car"' required :rules='[ruleVehicles]' />
          </v-col>
        </v-row>

        <v-row v-for="(n, i) in parseInt(numberOfPassengers)" :key="n">
          <v-col>
            <v-text-field v-model='passengers[i].firstName' :label='n == 1 ? `Hovedpassasjer fornavn` : `Passasjer #${n} fornavn`' required :rules='[ruleRequired, ruleName]' />
          </v-col>

          <v-col>
            <v-text-field v-model='passengers[i].lastName' :label='n == 1 ? `Hovedpassasjer etternavn` : `Passasjer #${n} etternavn`' required :rules='[ruleRequired, ruleName]' />
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field v-model='email' label='E-post' required :rules='[ruleRequired, ruleEmail]' type='email' prepend-inner-icon='mdi-email' />
          </v-col>

          <v-col>
            <v-text-field v-model='phone' type='number' min="10000000" label='Telefon' required :rules='[ruleRequired, rulePhone]' prepend-inner-icon='mdi-phone' />
          </v-col>
        </v-row>

        <address-input :street.sync="street" :zip.sync="zip" :rule-required="ruleRequired"></address-input>

        <v-row v-if="totalPrice" class="mb-4">
          <div>
            <h4>Pris per passasjer: {{ pricePerPassenger }},- kr</h4>
            <h4>Pris per kjøretøy: {{ pricePerVehicle }},- kr</h4>
            <h4>Total pris: {{ totalPrice }},- kr</h4>
          </div>
        </v-row>

        <v-row>
          <v-btn color="primary" @click='submitForm'>Send bestilling</v-btn>
        </v-row>
      </v-container>
    </v-form>
  </v-container>
</template>

<script>
import RouteSelector from "@/components/RouteSelector.vue";
import RouteTimesSelector from "@/components/RouteTimesSelector.vue";
import AddressInput from "@/components/AddressInput.vue";

export default {
  name: "Bestill",
  components: {
    RouteSelector,
    RouteTimesSelector,
    AddressInput,
  },
  data() {
    return {
      routeTimes: [],
      departureDate: null,
      returnDate: null,
      route: null,
      isRoundTrip: false,
      // form fields
      passengers: new Array(9)
        .fill()
        .map(() => ({ firstName: null, lastName: null })),
      numberOfPassengers: 1,
      numberOfVehicles: 0,
      email: null,
      phone: null,
      street: null,
      zip: null,
      //
      // rules used for input validaiton
      ruleRequired: v => !!v || "Required",
      rulePassengers: v =>
        (v >= 1 && v <= 9) || "Passasjerer må være mellom 1 og 9",
      ruleVehicles: v => (v >= 0 && v <= 9) || "Kjøretøy må være mellom 0 og 9",
      ruleName: v =>
        /^[a-zA-ZæøåÆØÅ .'-]{2,20}$/.test(v) ||
        "Name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters",
      ruleEmail: v => /.+@.+\..+/.test(v) || "Invalid email",
      rulePhone: v =>
        /^[0-9]{8}$/.test(v) || "Telefonnummeret må bestå av 8 tall",
      //
      isFormValid: false,
    };
  },
  computed: {
    // we have to 'reverse engineer' the route times using the selected dates because of limitaitons with the datepicker
    departureRouteTime() {
      return this.routeTimes.find(
        routeTime => routeTime.date.slice(0, 10) == this.departureDate
      );
    },
    returnRouteTime() {
      return this.routeTimes.find(
        routeTime => routeTime.date.slice(0, 10) == this.returnDate
      );
    },
    pricePerPassenger() {
      if (this.isRoundTrip && this.returnRouteTime != null) {
        return this.departureRouteTime?.price + this.returnRouteTime.price;
      } else {
        return this.departureRouteTime?.price;
      }
    },
    pricePerVehicle() {
      return 3 * this.pricePerPassenger;
    },
    totalPrice() {
      return (
        this.pricePerPassenger * parseInt(this?.numberOfPassengers) +
        this.pricePerVehicle * parseInt(this?.numberOfVehicles)
      );
    },
  },
  methods: {
    submitForm: function () {
      // validerer skjemaet
      this.$refs.form.validate();

      if (this.isFormValid) {
        fetch("/api/orders", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            mainCustomer: {
              ...this.passengers[0],
              email: this.email,
              phone: this.phone,
              street: this.street,
              zipCode: { id: this.zip },
            },
            customers: this.passengers.filter(
              (p, index) =>
                index != 0 && p.firstName != null && p.lastName != null
            ),
            departureRouteTime: {
              id: parseInt(this.departureRouteTime?.id),
            },
            returnRouteTime: {
              id: this.isRoundTrip
                ? parseInt(this.returnRouteTime?.id) || -1
                : -1,
            },
            numberOfVehicles: parseInt(this.numberOfVehicles),
            isRoundTrip: this.isRoundTrip,
          }),
        }).then(response => {
          console.log(response);
          if (!response.ok) {
            response.text().then(text => alert(text));
          } else {
            this.$router.push({
              name: "receipt",
              params: {
                origin: this.departureRouteTime.route.origin.name,
                destination: this.departureRouteTime.route.destination.name,
                company: this.departureRouteTime.route.company.name,
                departureDate: this.departureRouteTime.date,
                returnDate: this.returnRouteTime?.date,
                zipCode: this.zip,
                firstName: this.passengers[0].firstName,
                lastName: this.passengers[0].lastName,
                email: this.email,
                phone: this.phone,
                street: this.street,
                customers: this.passengers.filter(
                  (p, index) =>
                    index != 0 && p.firstName != null && p.lastName != null
                ),
                numberOfVehicles: this.numberOfVehicles,
                isRoundTrip: this.isRoundTrip,
                totalPrice: this.totalPrice,
              },
            });
          }
        });
      }
    },
  },
};
</script>
