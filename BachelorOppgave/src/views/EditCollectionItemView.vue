<template>
    <main class="flex-auto mx-auto max-w-3xl max-h-full overflow-y-scroll flex flex-col p-8 gap-8">
        <div class="flex flex-col gap-2">
            <h1 class="text-xl font-semibold">{{ formatDashCase(collectionItemSlug) }}</h1>
            <h3>Published: {{ collectionItemDate }}</h3>
        </div>
        <CMSContentEditor :collectionItem="collectionItem" :collection="collection"></CMSContentEditor>
        <div class="flex justify-between gap-2">
            <Button @click="deleteItem" label="Delete" class="p-button-danger"></Button>
            <Button @click="saveItem" label="Save"></Button>
        </div>
    </main>
</template>

<script setup>
import { reactive } from "vue"
import { useRoute } from "vue-router"
import CMSContentEditor from "../components/CMSContentEditor.vue"
import { useToast } from "primevue/usetoast";
import router from "../router"
import { store } from "../store"

const { OWNER, REPO, accessToken, user } = store

const toast = useToast();
const route = useRoute()
const { collection, collectionItem: paramCollectionItem } = route.params

const collectionItemDate = paramCollectionItem.slice(0, 10)     // henter ut datoen fra filnavnet/route params
const collectionItemSlug = paramCollectionItem.slice(11)        // henter ut slug fra filnavnet/route params

const PATH = `data/${collection}/${paramCollectionItem}.json`    // stien til innholdet vi skal hente/endre

const response = await fetch(`https://api.github.com/repos/${OWNER}/${REPO}/contents/${PATH}`, {
    headers: {
        Authorization: `Bearer ${accessToken}`
    },
})

const data = await response.json()

let { sha } = data                              // filnavn og sha-verdi til innholdet i GitHub-repositoriet vi skal oppdatere
const decodedData = atob(data.content)          // content er base64 enkodet så vi må dekode med atob-funksjonen
const parsedData = JSON.parse(decodedData)      // så parser vi til JSON så vi kan jobbe med objektet
const collectionItem = reactive(parsedData)     // gjør objektet reaktivt så vi kan endre data i GUI 

const saveItem = (async () => {
    const response = await fetch(`https://api.github.com/repos/${OWNER}/${REPO}/contents/${PATH}`, {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
        method: "PUT",
        body: JSON.stringify({
            message: `Update ${collectionItem.date}-${collectionItem.slug} in ${collection}`,
            sha,
            content: btoa(unescape(encodeURIComponent(JSON.stringify(collectionItem))))
        })
    })

    const data = await response.json()
    sha = data.content.sha      // må oppdatere sha sjekksummen for å kunne commit på nytt uten å laste siden på nytt
    toast.add({ severity: "info", summary: "Changes saved", detail: `Successfully saved changes to "${formatDashCase(collectionItemSlug)}"`, life: 5000 });
})

const deleteItem = (async () => {
    const response = await fetch(`https://api.github.com/repos/${OWNER}/${REPO}/contents/${PATH}`, {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
        method: "DELETE",
        body: JSON.stringify({
            message: `Delete ${collectionItem.date}-${collectionItem.slug} from ${collection}`,
            sha,
        })
    })

    // const data = await response.json()
    router.push(`/collections/${collection}/`)
    toast.add({ severity: "info", summary: "Item deleted", detail: `Successfully deleted "${formatDashCase(collectionItemSlug)}"`, life: 5000 });
    setTimeout(() => {
        delete store.collections[collection][paramCollectionItem]   // sletter elementet fra valgte collection
    }, 500)
})

const formatDashCase = (text) => {
    const formattedText = text.replaceAll("-", " ")
    return formattedText[0].toUpperCase() + (formattedText.slice(1)).toLowerCase()
}
</script>
