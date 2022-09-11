<template>
    <div class="flex flex-col gap-2" v-for="(value, key) in collectionItem">
        <label :for="key">{{ formatDashCase(key) }}</label>

        <template v-if="fields[key] == 'string'">
            <InputText :name="key" :id="key" v-model="collectionItem[key]" />
        </template>
        <template v-else-if="fields[key] == 'text'">
            <InputText :name="key" :id="key" v-model="collectionItem[key]" />
        </template>
        <template v-else-if="fields[key] == 'image'">
            <div class="flex gap-4">
                <FileUpload :name="key" mode="basic" :auto="true" accept="image/*" chooseLabel="Upload" :customUpload="true" @uploader="uploadImage">
                    <template #empty>
                        <p>Drag and drop file to here to upload.</p>
                    </template>
                </FileUpload>
                <Button v-if="collectionItem[key]" @click="removeImage" label="Remove" class="p-button-danger"></Button>
            </div>
            <Image v-if="collectionItem[key]" :src="collectionItem[key]" preview class="max-h-100 !flex justify-center" imageClass="max-w-full max-h-full" />
        </template>
        <template v-else-if="fields[key] == 'datetime'">
            <InputText :name="key" :id="key" v-model="collectionItem[key]" type="datetime-local" />
        </template>
        <template v-else-if="fields[key] == 'markdown'">
            <QuillEditor :content="htmlContent(value)" @update:content="(content) => contentChange(content, key)" contentType="html" theme="snow" />
        </template>
        <template v-else-if="fields[key] == 'array'">
            <Dropdown :name="key" :id="key" v-model="collectionItem[key]" placeholder="Select one" />
        </template>

        <p v-if="descriptions[key]" class="text-gray-500">{{ descriptions[key] }}</p>
    </div>
</template>

<script setup>
import { store } from "../store"
import showdown from "showdown"

const { schemas } = store

const converter = new showdown.Converter()

const { collection, collectionItem } = defineProps(["collection", "collectionItem"])
const schema = schemas[collection]   // henter schema for den collection vi jobber med, brukes til å bestemme felttyper og vise beskrivelser

// lager object med format { "title": "string", "date": "datetime" }
const fields = schema.reduce((a, v) => ({ ...a, [v.id]: v.type }), {})

// lager objekt med format { "title": "", "slug": "Used in the URL ..." }
const descriptions = schema.reduce((a, v) => ({ ...a, [v.id]: v.description || "" }), {})


const uploadImage = ({ files }) => {
    console.log(files[0])

    const reader = new FileReader();
    reader.onloadend = () => {
        const base64Image = reader.result;
        collectionItem.image = base64Image
    }

    (async () => {
        const response = await fetch(files[0].objectURL)
        const blob = await response.blob()
        reader.readAsDataURL(blob)
    })()
}

const removeImage = () => {
    collectionItem.image = ""
}

const htmlContent = (content) => {
    return converter.makeHtml(content)      // konverterer markdown fra databasen til html så det vises riktig i editoren
}

const contentChange = (content, key) => {
    collectionItem[key] = converter.makeMarkdown(content).replaceAll("\n\n\n", "\n\n").replaceAll("\n\n", "\n")   // konverterer html fra editoren til markdown så det lagres riktig
}

const formatDashCase = (text) => {
    const formattedText = text.replaceAll("-", " ")
    return formattedText[0].toUpperCase() + (formattedText.slice(1)).toLowerCase()
}
</script>

<style>
/* datepicker vil egentlig fylle bredden til input feltet men det ser rart ut på store skjermer så vi begrenser den */
.p-datepicker {
    min-width: auto !important;
}
</style>