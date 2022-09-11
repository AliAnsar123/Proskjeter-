import { createApp } from "vue"
import App from "./App.vue"
import router from "./router"
import "virtual:windi.css"
import PrimeVue from "primevue/config"
import Button from "primevue/button"
import InputText from "primevue/inputtext"
import Calendar from "primevue/calendar"
import FileUpload from "primevue/fileupload"
import Image from "primevue/image"
import Dropdown from "primevue/dropdown"
import ToastService from "primevue/toastservice"
import Toast from "primevue/toast"
import { QuillEditor } from "@vueup/vue-quill"

// import "primevue/resources/themes/saga-blue/theme.css"
import "./assets/theme.css"
import "primevue/resources/primevue.min.css"
import "primeicons/primeicons.css"

import "@vueup/vue-quill/dist/vue-quill.snow.css"


const app = createApp(App)

app.use(router)
    .use(PrimeVue, { ripple: true })
    .use(ToastService)
    .component("Button", Button)
    .component("InputText", InputText)
    .component("Calendar", Calendar)
    .component("FileUpload", FileUpload)
    .component("Image", Image)
    .component("Dropdown", Dropdown)
    .component("Toast", Toast)
    .component("QuillEditor", QuillEditor)
    .mount("#app")
