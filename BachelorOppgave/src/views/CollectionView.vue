<template>
    <nav class="h-auto max-w-full lg:max-w-lg max-h-screen overflow-initial lg:overflow-auto bg-gray-100 flex flex-rows flex-wrap lg:flex-nowrap lg:flex-col">
        <div class="w-full lg:w-auto top-0 sticky bg-gray-100 px-4 pt-4 lg:pt-6 flex items-center justify-between">
            <h1 class="text-xl font-semibold">{{ formatDashCase(route.params.collection) }}</h1>
            <RouterLink to="create" class="">
                <Button label="Create new" class="w-full"></Button>
            </RouterLink>
        </div>

        <div class="w-full flex gap-4 overflow-x-scroll lg:flex-col lg:overflow-auto p-4 lg:p-4">
            <RouterLink v-for="{ date, slug } in store.collections[collection]" :to="`/collections/${collection}/${date}-${slug}`" active-class="font-semibold" class="max-w-xs lg:max-w-md">
                <p class="truncate">{{ formatDashCase(slug) }}</p>
                <p class="text-gray-600">Published: {{ date }}</p>
            </RouterLink>
        </div>
    </nav>

    <Suspense>
        <template #default>
            <RouterView :key="$route.fullPath" />
        </template>
        <template #fallback>
            <div class="mx-auto h-screen flex justify-center items-center">Loading...</div>
        </template>
    </Suspense>
</template>

<script setup>
import { RouterLink, RouterView, useRoute } from "vue-router";
import { store } from "../store"

const { OWNER, REPO, accessToken, schemas } = store

const route = useRoute()
const { collection } = route.params

// henter alle items (f.eks. nyhetsartiklers) tittel og dato
const collectionItemsResponse = await fetch(`https://api.github.com/repos/${OWNER}/${REPO}/contents/data/${collection}`, {
    headers: {
        Authorization: `Bearer ${accessToken}`
    },
    cache: "no-cache"   // denne sikrer at vi laster inn nyeste data umiddelbart
})

const collectionItems = await collectionItemsResponse.json()
store.collections[collection] = collectionItems.reduce((a, v) => ({
    [v.name.slice(0, -5)]: {            // slice fjerner .json i filnavnet
        date: v.name.slice(0, 10),      // name har format "YYYY-MM-DD-slug.json"
        slug: v.name.slice(11, -5),     // med slice henter vi ut dato og slug
    },
    ...a
}), {})

// henter hele schema for den gitte collection, felt med navn, type og beskrivelser  
const collectionSchemaResponse = await fetch(`https://api.github.com/repos/${OWNER}/${REPO}/contents/collections/${collection}.json`, {
    headers: {
        Authorization: `Bearer ${accessToken}`
    },
})
const collectionSchemaData = await collectionSchemaResponse.json()
const decodedData = atob(collectionSchemaData.content)
const parsedData = JSON.parse(decodedData)

schemas[collection] = parsedData.fields

const formatDashCase = (text, capitalize = true) => {
    const formattedText = text.replaceAll("-", " ")
    return capitalize ? formattedText[0].toUpperCase() + (formattedText.slice(1)).toLowerCase() : formattedText
}
</script>