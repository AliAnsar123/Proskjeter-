<template>
    <nav class="lg:max-w-xs lg:w-12rem lg:h-screen px-8 py-4 lg:py-8 bg-gray-200 flex gap-6 flex-rows lg:flex-col items-center lg:items-start">
        <RouterLink to="/">
            <img src="../assets/piql-logo.png" class="w-18 lg:w-24" alt="Piql logo">
        </RouterLink>

        <RouterLink v-for="(collectionItems, collection) in store.collections" :to="`/collections/${collection}/`" active-class="font-semibold" class="text-lg">
            {{ formatDashCase(collection) }}
        </RouterLink>

        <RouterLink v-if="store.accessToken" to="/settings" active-class="font-semibold" class="text-lg ml-auto lg:ml-0 mt-0 lg:mt-auto">Settings</RouterLink>
    </nav>
</template>

<script setup>
import { RouterLink } from "vue-router"
import { store } from "../store/index.js";

const formatDashCase = (text) => {
    text = text.replaceAll("-", " ")
    return text[0].toUpperCase() + (text.slice(1)).toLowerCase()
}
</script>