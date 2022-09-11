import { reactive } from "vue"
import { useStorage } from "@vueuse/core"

export const store = reactive({
    accessToken: useStorage("accessToken", ""),
    collections: {},
    schemas: {},
    OWNER: "LavransBjerkestrand",           // eieren av GitHub-repoet hvor CMS dataen lagres
    REPO: "piql-cms",                       // GitHub-repoet hvor CMS dataen lagres
    SCOPE: "repo",                          // Bestemmer hvilke ressurser access token vi får har tilgang til - https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
    CLIENT_ID: "07b32cbcf978c7d68247",      // client_id-en til GitHub OAuth-Appen vår
    CLIENT_SECRET: "4023c126ff8a46fd2b6dd408ff088a5b60b0e5a5",
})