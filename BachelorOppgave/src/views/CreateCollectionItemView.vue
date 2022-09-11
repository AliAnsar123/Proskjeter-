<template>
    <main class="flex-auto mx-auto max-w-3xl max-h-screen overflow-y-scroll flex flex-col p-8 gap-8">
        <div class="flex flex-col gap-2">
            <h1 class="text-xl font-semibold">Create new</h1>
        </div>
        <CMSContentEditor :collectionItem="collectionItem" :collection="collection"></CMSContentEditor>
        <div class="flex justify-between">
            <Button @click="saveItem" label="Save"></Button>
        </div>
    </main>
</template>

<script setup>
import { reactive } from "vue"
import { useRoute } from "vue-router"
import { useToast } from "primevue/usetoast";
import CMSContentEditor from "../components/CMSContentEditor.vue"
import router from "../router"
import { store } from "../store"

const { OWNER, REPO, accessToken } = store

const toast = useToast();
const route = useRoute()
const { collection } = route.params

const schema = store.schemas[collection]

const collectionItemObject = schema.reduce((a, v) => ({ ...a, [v.id]: "" }), {}) // lager object med format { "title": "", "date": "", ... }
const collectionItem = reactive(collectionItemObject)   // gjør objektet reaktivt så vi kan endre data i GUI 

const saveItem = (async () => {
    // stien til filen vi skal opprette
    const PATH = `data/${collection}/${collectionItem.date.split("T")[0]}-${collectionItem.slug}.json`

    const response = await fetch(`https://api.github.com/repos/${OWNER}/${REPO}/contents/${PATH}`, {
        headers: {
            Authorization: `Bearer ${accessToken}`,
        },
        method: "PUT",
        body: JSON.stringify({
            message: `Create ${collectionItem.date}-${collectionItem.slug} in ${collection}`,
            content: btoa(unescape(encodeURIComponent(JSON.stringify(collectionItem))))   // gjøre objektet til en JSON-string og koder den til base64 format
        })
    })

    console.log(response)

    const data = await response.json()
    console.log(data)
    router.push(`/collections/${collection}/`)
    toast.add({ severity: "info", summary: "Item created", detail: `Successfully created "${collectionItem.title}"`, life: 5000 });
})

</script>
