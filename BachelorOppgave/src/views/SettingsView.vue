<template>
  <div class="w-15rem text-center mx-auto h-screen flex flex-col justify-center items-center gap-12">
    <h1 class="text-2xl">CMS settings</h1>
    <div class="flex flex-col items-center gap-2">
      <a target="nofollow" :href="`https://github.com/${OWNER}/${REPO}`"><Button class="w-15rem" label="GitHub Repository"></Button></a>
      <p class="text-gray-600">Link to the CMS content repository on GitHub</p>
    </div>

    <div class="flex flex-col items-center gap-2">
      <a target="nofollow" :href="`https://github.com/${OWNER}/${REPO}/commits`"><Button class="w-15rem" label="Commit history"></Button></a>
      <p class="text-gray-600">Link to the CMS content repositorys commit history/logs</p>
    </div>

    <div class="flex flex-col items-center gap-2">
      <a target="nofollow" :href="`https://github.com/${OWNER}/${REPO}/settings/access`"><Button class="w-15rem" label="Manage repository access"></Button></a>
      <p class="text-gray-600">Link to the CMS content repositorys access settings</p>
    </div>

    <Button class="w-15rem" @click="clearData" label="Clear local data"></Button>

    <Button class="w-15rem p-button-danger" @click="logout" label="Logout"></Button>
  </div>
</template>

<script setup>
import router from "../router/index.js"
import { store } from "../store"
const { OWNER, REPO } = store

const clearData = () => {
  store.collections = {}
  store.schemas = {}
}

const logout = () => {
  clearData()
  store.accessToken = ""
  router.push("/login")
}
</script>