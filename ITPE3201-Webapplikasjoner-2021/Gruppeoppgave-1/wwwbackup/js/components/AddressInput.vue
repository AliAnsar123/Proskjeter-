<template>
    <v-row>
        <v-col>
            <v-text-field
                :value='street'
                @input='$emit("update:street", $event)'
                label='Gateadresse'
                required
                :rules='[ruleRequired]'
            />
        </v-col>
        <v-col>
            <v-text-field
                :value='zip'
                @input='$emit("update:zip", $event)'
                label='Postkode'
                required
                :rules='[ruleRequired, ...rulesZipCode]'
                type='number'
                min='0001'
                max='9999'
            />
        </v-col>

        <v-col>
            <v-text-field
                :value='city'
                label='Poststed'
                readonly
            />
        </v-col>
    </v-row>
</template>

<script>
module.exports = {
    name: 'AddressInput',
    props: {
        street: String,
        zip: String,
        ruleRequired: Function,
    },
    data() {
        return {
            zipCodes: [],
        }
    },
    computed: {
        city() {
            const city = this.zipCodes[this.zip]
            this.$emit('update:city', city)
            return city
        },
        // used for validation of zip code input field
        rulesZipCode() {
            return [
                v => /^\d{4}$/.test(v) || 'Ugyldig postkode',
                _ => !!this.city || 'Ukjent postkode'
            ]
        },
    },
    mounted() {
        fetch('/zipCodes')
            .then(response => response.json())
            .then(zipCodes => zipCodes.forEach(zipCode => this.zipCodes[zipCode.id] = zipCode.city))
    }
}
</script>