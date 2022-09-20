<template>
    <v-app>
        <v-app-bar app>
            <v-container class='py-0 fill-height'>
                <router-link to="/order">Bestill båttur</router-link>
                <router-link to="/admin">Admin</router-link>
            </v-container>
        </v-app-bar>
        <v-main>
            <v-container v-if='showForm' class='my-5'>
                <v-form
                    v-model='isFormValid' 
                    ref='form'>
                    <v-container>
                        <h1>Hvor vil du reise?</h1>
                        
                        <route-selector
                            :is-round-trip.sync="isRoundTrip"
                            :route.sync="route"
                            :rule-required="ruleRequired">
                        </route-selector>

                        <route-times-selector
                            :departure-date.sync="departureDate"
                            :return-date.sync="returnDate"
                            :is-round-trip="isRoundTrip"
                            :route="route"
                            :route-times.sync="routeTimes"
                            :rule-required="ruleRequired">
                        </route-times-selector>

                        <v-row>
                            <v-col>
                                <v-text-field
                                    v-model='passengers'
                                    type='number'
                                    label='Passasjerer'
                                    min='1'
                                    max='9'
                                    :prepend-inner-icon='(passengers > 1) ? "mdi-account-multiple" : "mdi-account"'
                                    required
                                    :rules='[ruleRequired, rulePassengers]'
                                />
                            </v-col>
                            <v-col>
                                <v-text-field
                                    v-model='vehicles'
                                    type='number'
                                    label='Kjøretøy'
                                    min='0'
                                    max='9'
                                    :prepend-inner-icon='(vehicles > 1) ? "mdi-car-multiple" : "mdi-car"'
                                    required
                                    :rules='[ruleVehicles]'
                                />
                            </v-col>
                        </v-row>

                        <v-row>
                            <v-col>
                                <v-text-field
                                    v-model='firstName'
                                    label='Fornavn'
                                    required
                                    :rules='[ruleRequired, ruleName]'
                                />
                            </v-col>

                            <v-col>
                                <v-text-field
                                    v-model='lastName'
                                    label='Etternavn'
                                    required
                                    :rules='[ruleRequired, ruleName]'
                                />
                            </v-col>
                        </v-row>

                        <v-row>
                            <v-col>
                                <v-text-field
                                    v-model='email'
                                    label='E-post'
                                    required
                                    :rules='[ruleRequired, ruleEmail]'
                                    type='email'
                                    prepend-inner-icon='mdi-email'
                                />
                            </v-col>

                            <v-col>
                                <v-text-field
                                    v-model='phone'
                                    type='number'
                                    min="10000000"
                                    label='Telefon'
                                    required
                                    :rules='[ruleRequired, rulePhone]'
                                    prepend-inner-icon='mdi-phone'
                                />
                            </v-col>
                        </v-row>

                        <address-input
                            :street.sync="street"
                            :zip.sync="zip"
                            :rule-required="ruleRequired"
                        ></address-input>

                        <v-row v-if="totalPrice" class="mb-4">
                            <div>
                                <h4>Pris per passasjer: {{ pricePerPassenger }},- kr</h4>
                                <h4>Pris per kjøretøy: {{ pricePerVehicle }},- kr</h4>
                                <h4>Total pris: {{ totalPrice }},- kr</h4>
                            </div>
                        </v-row>

                        <v-row>
                            <v-btn @click='submitForm'>Send bestilling</v-btn>
                        </v-row>
                    </v-container>
                </v-form>
            </v-container>
            
            <v-col v-if="!showForm" class="my-4">
                <h1>Alle ordre</h1>
                <orders-table></orders-table>
            </v-col>
        </v-main>
    </v-app>
</template>

<script>
module.exports = {
    components: {
        routeSelector:      httpVueLoader('./components/RouteSelector.vue'),
        routeTimesSelector: httpVueLoader('./components/RouteTimesSelector.vue'),
        addressInput:       httpVueLoader('./components/AddressInput.vue'),
        ordersTable:        httpVueLoader('./components/OrdersTable.vue'),
    },
    data() {
        return {
            showForm: true,
            routeTimes: [],
            departureDate: null,
            returnDate: null,
            route: null,
            isRoundTrip: false,
            // form fields
            passengers: 1,
            vehicles: 0,
            firstName: null,
            lastName: null,
            email: null,
            phone: null,
            street: null,
            zip: null,
            //
            // rules used for input validaiton
            ruleRequired: v => !!v || 'Påkrevd',
            rulePassengers: v => (v >= 1 && v <= 9) || 'Passasjerer må være mellom 1 og 9',
            ruleVehicles: v => (v >= 0 && v <= 9) || 'Kjøretøy må være mellom 0 og 9',
            ruleName: v => /^[a-zA-ZæøåÆØÅ .'-]{2,20}$/.test(v) || 'Navn kan bare inneholde bokstaver, punktum, bindestrek og apostrof, og må være mellom 2 og 20 tegn.',
            ruleEmail: v => /.+@.+\..+/.test(v) || 'Ugyldig e-post',
            rulePhone: v => /^[0-9]{8}$/.test(v) || 'Telefonnummeret må bestå av 8 tall',
            //
            isFormValid: false,
        }
    },
    computed: {
        // we have to 'reverse engineer' the route times using the selected dates because of limitaitons with the datepicker
        departureRouteTime() {
            return this.routeTimes.find(routeTime => routeTime.date.slice(0, 10) == this.departureDate)
        },
        returnRouteTime() {
            return this.routeTimes.find(routeTime => routeTime.date.slice(0, 10) == this.returnDate)
        },
        pricePerPassenger() {
            return this.returnRouteTime != null ? this.departureRouteTime?.price + this.returnRouteTime.price : this.departureRouteTime?.price
        },
        pricePerVehicle() {
            return 3 * this.pricePerPassenger
        },
        totalPrice() {
            return this.pricePerPassenger * parseInt(this.passengers) + this.pricePerVehicle * parseInt(this.vehicles)
        }
    },
    methods: {
        submitForm: function () {
            // validerer skjemaet
            this.$refs.form.validate()

            if (this.isFormValid) {
                fetch('/orders', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        customer: {
                            firstName: this.firstName,
                            lastName: this.lastName,
                            email: this.email,
                            phone: parseInt(this.phone),
                            street: this.street,
                            zipCode: {
                                id: this.zip
                            }
                        },
                        departureRouteTime: {
                            id: parseInt(this.departureRouteTime?.id)
                        },
                        returnRouteTime: {
                            id: parseInt(this.returnRouteTime?.id || 0)
                        },
                        passengers: parseInt(this.passengers),
                        vehicles: parseInt(this.vehicles),
                        isRoundTrip: this.isRoundTrip
                    })
                }).then(response => {
                    console.log(response)
                    if (!response.ok) {
                        response.text().then(text => alert(text))
                    } else {
                        alert('Ordre sendt')
                        location.reload()
                    }
                })
            }
        }
    }
}
</script>