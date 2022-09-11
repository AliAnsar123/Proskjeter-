import { createRouter, createWebHistory } from "vue-router"
import { store } from "../store";

import HomeView from "../views/HomeView.vue"
import LoginView from "../views/LoginView.vue"
import SettingsView from "../views/SettingsView.vue"
import CollectionView from "../views/CollectionView.vue"
import EditCollectionItemView from "../views/EditCollectionItemView.vue"
import CreateCollectionItemView from "../views/CreateCollectionItemView.vue"

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      component: HomeView
    },
    {
      path: "/login",
      component: LoginView
    },
    {
      path: "/settings",
      component: SettingsView
    },
    {
      path: "/collections/:collection",
      component: CollectionView,
      children: [
        {
          path: "create",
          component: CreateCollectionItemView
        },
        {
          path: ":collectionItem",
          component: EditCollectionItemView
        },
      ],
    },
  ]
})

router.beforeEach(async (to) => {
  const { accessToken } = store
  const isLoggedIn = Boolean(accessToken) // hvis brukeren har access token kan vi anta at hen er logget inn

  // hvis brukeren ikke er logget inn sender hen til login
  if (to.path === "/" && !isLoggedIn) {
    return "/login"
  }

  // hvis brukeren er logget inn sender til startsiden i stedet for login
  if (to.path === "/login" && isLoggedIn) {
    return "/"
  }

  // sjekk om token er gyldig
  if (!to.path.includes("/login")) {
    const response = await fetch("https://api.github.com/", {
      headers: {
        Authorization: `Bearer ${accessToken}`
      },
    })

    // hvis token ikke er gyldig sletter vi den og collections og router til login
    if (!response.ok) {
      store.accessToken = ""
      store.collections = []
      return "/login"
    }
  }

  // hvis stien inneholder collections henter vi collection items for den valgte collection
  if (to.path.includes("/collections/")) {
    const { collection } = to.params

    console.log("collection:", collection);
  }
})

export default router
