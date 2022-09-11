<template>
    <div class="h-full flex flex-col justify-center items-center mx-auto gap-8">
        <Button>
            <a class="text-xl" :href="`https://github.com/login/oauth/authorize?client_id=${CLIENT_ID}&scope=${SCOPE}`">Login with GitHub</a>
        </Button>
        <p class="text-lg">You need to login to access the CMS</p>
        <!-- <pre>Code: {{ $route.query.code }}</pre> -->
    </div>
</template>

<script setup>
import { onMounted } from "vue"
import { useRoute } from "vue-router"
import { store } from "../store"
import router from "../router"

const { CLIENT_ID, CLIENT_SECRET, SCOPE } = store


onMounted(async () => {
    const route = useRoute()

    // hvis brukeren logget inn hos GitHub får vi code parameteret tilbake som vi bruker for å få access token
    if (route.query.code) {
        const response = await fetch(`https://github.com/login/oauth/access_token?client_id=${CLIENT_ID}&client_secret=${CLIENT_SECRET}&code=${route.query.code}`, {
            method: "POST",
        })
        const data = await response.text()      // kommer i format "access_token=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&scope=repo&token_type=bearer"
        const token = data.slice(13, 53)        // henter ut selve access token "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"

        if (token.slice(0, 4) != "gho_") {      // hvis token ikke har riktig format gikk noe galt
            alert(`Noe gikk galt: ${data}`)     // error=bad_verification_code&error_description=The+code+passed+is+incorrect+or+expired...
        } else {
            store.accessToken = token
            router.go()     // laster siden på nytt så vi blir sendt til startskjermen med nye data
        }
    }
})
</script>