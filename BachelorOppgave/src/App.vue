<template>
  <Toast />

  <div class="flex w-full h-screen flex-col lg:flex-row">
    <NavBar></NavBar>

    <Suspense>
      <template #default>
        <RouterView :key="$route.path" />
      </template>
      <template #fallback>
        <div>Loading...</div>
      </template>
    </Suspense>
  </div>
</template>

<script setup>
import { RouterView } from "vue-router"
import { onMounted } from "vue"
import { store } from "./store/index.js"
import router from "./router/index.js"
import NavBar from "./components/NavBar.vue"

const { OWNER, REPO, accessToken } = store

onMounted(async () => {
  // hvis vi ikke har access token er vi på loginskjermen og det er ikke vits å prøve å hente collections
  if (!accessToken) return

  const PATH = "collections"            // stien til innholdet vi skal ha tak i

  const response = await fetch(`https://api.github.com/repos/${OWNER}/${REPO}/contents/${PATH}`, {
    headers: {
      Authorization: `Bearer ${accessToken}`
    },
  })

  if (!response.ok) {       // hvis forespørsel ikke ble godtatt er trolig access_token utløpt
    store.accessToken = ""  // vi sletter da access_token
    router.push("/login")   // og ber brukeren logge inn på nytt
    return
  }

  const data = await response.json()
  store.collections = data.reduce((a, v) => ({ ...a, [v.name.slice(0, -5)]: [] }), {})    // henter ut bare filnavnet fjerner ".json" fra strengen
})
</script>