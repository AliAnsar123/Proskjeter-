(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-3e5fdd2a"],{"1f4f":function(e,t,s){"use strict";var r=s("5530"),a=(s("a9e3"),s("8b37"),s("80d2")),i=s("7560"),n=s("58df");t["a"]=Object(n["a"])(i["a"]).extend({name:"v-simple-table",props:{dense:Boolean,fixedHeader:Boolean,height:[Number,String]},computed:{classes:function(){return Object(r["a"])({"v-data-table--dense":this.dense,"v-data-table--fixed-height":!!this.height&&!this.fixedHeader,"v-data-table--fixed-header":this.fixedHeader,"v-data-table--has-top":!!this.$slots.top,"v-data-table--has-bottom":!!this.$slots.bottom},this.themeClasses)}},methods:{genWrapper:function(){return this.$slots.wrapper||this.$createElement("div",{staticClass:"v-data-table__wrapper",style:{height:Object(a["e"])(this.height)}},[this.$createElement("table",this.$slots.default)])}},render:function(e){return e("div",{staticClass:"v-data-table",class:this.classes},[this.$slots.top,this.genWrapper(),this.$slots.bottom])}})},"2a7f":function(e,t,s){"use strict";s.d(t,"a",(function(){return i}));var r=s("71d9"),a=s("80d2"),i=Object(a["g"])("v-toolbar__title"),n=Object(a["g"])("v-toolbar__items");r["a"]},"8b37":function(e,t,s){},efce:function(e,t,s){"use strict";s.r(t);var r=function(){var e=this,t=e.$createElement,s=e._self._c||t;return e.isLoading?e._e():s("v-container",{staticClass:"my-5",staticStyle:{"max-width":"32rem"}},[s("v-form",{ref:"form",model:{value:e.isFormValid,callback:function(t){e.isFormValid=t},expression:"isFormValid"}},[s("v-simple-table",{attrs:{dense:""},scopedSlots:e._u([{key:"top",fn:function(){return[s("v-toolbar",{attrs:{flat:""}},[s("v-toolbar-title",[e._v("Create new customer")])],1)]},proxy:!0}],null,!1,2403791785)},[s("tbody",[s("tr",[s("th",[e._v("Property")]),s("th",[e._v("Value")])]),s("tr",[s("td",[e._v("First name")]),s("td",[s("v-text-field",{attrs:{rules:[e.ruleRequired,e.ruleName]},model:{value:e.customer.firstName,callback:function(t){e.$set(e.customer,"firstName",t)},expression:"customer.firstName"}})],1)]),s("tr",[s("td",[e._v("Last name")]),s("td",[s("v-text-field",{attrs:{rules:[e.ruleRequired,e.ruleName]},model:{value:e.customer.lastName,callback:function(t){e.$set(e.customer,"lastName",t)},expression:"customer.lastName"}})],1)]),s("tr",[s("td",[e._v("Email")]),s("td",[s("v-text-field",{attrs:{rules:[e.ruleRequired,e.ruleEmail]},model:{value:e.customer.email,callback:function(t){e.$set(e.customer,"email",t)},expression:"customer.email"}})],1)]),s("tr",[s("td",[e._v("Phone")]),s("td",[s("v-text-field",{attrs:{rules:[e.ruleRequired,e.rulePhone]},model:{value:e.customer.phone,callback:function(t){e.$set(e.customer,"phone",t)},expression:"customer.phone"}})],1)]),s("tr",[s("td",[e._v("Street")]),s("td",[s("v-text-field",{attrs:{rules:[e.ruleRequired]},model:{value:e.customer.street,callback:function(t){e.$set(e.customer,"street",t)},expression:"customer.street"}})],1)]),s("tr",[s("td",[e._v("Zip code")]),s("td",[s("v-text-field",{attrs:{rules:[e.ruleRequired].concat(e.rulesZipCode),type:"number",min:"0001",max:"9999"},model:{value:e.customer.zipCode.id,callback:function(t){e.$set(e.customer.zipCode,"id",t)},expression:"customer.zipCode.id"}})],1)]),s("tr",[s("td",[e._v("City")]),s("td",[e._v(" "+e._s(e.customer.zipCode.city)+" ")])])])])],1),s("v-row",{staticClass:"pa-5",attrs:{justify:"space-between"}},[s("v-btn",{staticClass:"error",on:{click:e.cancel}},[e._v("Cancel")]),s("v-btn",{staticClass:"success",attrs:{disabled:!e.isFormValid},on:{click:e.save}},[e._v("Save")])],1)],1)},a=[],i=s("1da1"),n=(s("96cf"),s("ac1f"),s("00b4"),s("d3b7"),s("e9c4"),s("159b"),{name:"CreateCustomer",data:function(){return{customer:Object,zipCodes:{},isFormValid:Boolean,isLoading:!0,ruleRequired:function(e){return!!e||"Required"},ruleName:function(e){return/^[a-zA-ZæøåÆØÅ .'-]{2,20}$/.test(e)||"Name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters"},ruleEmail:function(e){return/.+@.+\..+/.test(e)||"Invalid email"},rulePhone:function(e){return/^[0-9]{8}$/.test(e)||"Phone must consist of 8 numbers"}}},watch:{"customer.zipCode.id":function(){this.customer.zipCode.city=this.zipCodes[this.customer.zipCode.id]}},computed:{rulesZipCode:function(){var e=this;return[function(e){return/^\d{4}$/.test(e)||"Ugyldig postkode"},function(){return!!e.customer.zipCode.city||"Ukjent postkode"}]}},methods:{cancel:function(){this.$router.push("/admin")},save:function(){var e=Object(i["a"])(regeneratorRuntime.mark((function e(){var t,s;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(this.$refs.form.validate(),!this.isFormValid){e.next=14;break}return e.next=4,fetch("/api/customers",{method:"POST",headers:{"Content-Type":"application/json"},body:JSON.stringify(this.customer)});case 4:if(t=e.sent,!t.ok){e.next=9;break}this.$router.push("/admin"),e.next=14;break;case 9:return e.next=11,t.text();case 11:s=e.sent,alert(s),"Unauthorized"==s&&this.$router.push("/login");case 14:case"end":return e.stop()}}),e,this)})));function t(){return e.apply(this,arguments)}return t}()},mounted:function(){var e=this;fetch("/api/zipcodes").then((function(e){return e.json()})).then((function(t){t.forEach((function(t){return e.zipCodes[t.id]=t.city})),e.isLoading=!1}))}}),o=n,u=s("2877"),c=s("6544"),l=s.n(c),d=s("8336"),m=s("a523"),f=s("4bd4"),h=s("0fd9"),p=s("1f4f"),v=s("8654"),b=s("71d9"),x=s("2a7f"),C=Object(u["a"])(o,r,a,!1,null,null,null);t["default"]=C.exports;l()(C,{VBtn:d["a"],VContainer:m["a"],VForm:f["a"],VRow:h["a"],VSimpleTable:p["a"],VTextField:v["a"],VToolbar:b["a"],VToolbarTitle:x["a"]})}}]);
//# sourceMappingURL=chunk-3e5fdd2a.8ca7aa58.js.map