(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-3b980b4b"],{"1f4f":function(t,e,a){"use strict";var n=a("5530"),r=(a("a9e3"),a("8b37"),a("80d2")),s=a("7560"),i=a("58df");e["a"]=Object(i["a"])(s["a"]).extend({name:"v-simple-table",props:{dense:Boolean,fixedHeader:Boolean,height:[Number,String]},computed:{classes:function(){return Object(n["a"])({"v-data-table--dense":this.dense,"v-data-table--fixed-height":!!this.height&&!this.fixedHeader,"v-data-table--fixed-header":this.fixedHeader,"v-data-table--has-top":!!this.$slots.top,"v-data-table--has-bottom":!!this.$slots.bottom},this.themeClasses)}},methods:{genWrapper:function(){return this.$slots.wrapper||this.$createElement("div",{staticClass:"v-data-table__wrapper",style:{height:Object(r["e"])(this.height)}},[this.$createElement("table",this.$slots.default)])}},render:function(t){return t("div",{staticClass:"v-data-table",class:this.classes},[this.$slots.top,this.genWrapper(),this.$slots.bottom])}})},"2a7f":function(t,e,a){"use strict";a.d(e,"a",(function(){return s}));var n=a("71d9"),r=a("80d2"),s=Object(r["g"])("v-toolbar__title"),i=Object(r["g"])("v-toolbar__items");n["a"]},"47fe":function(t,e,a){"use strict";a.r(e);var n=function(){var t=this,e=t.$createElement,a=t._self._c||e;return t.isLoading?t._e():a("v-container",{staticClass:"my-5",staticStyle:{"max-width":"32rem"}},[a("v-form",{ref:"form",model:{value:t.isFormValid,callback:function(e){t.isFormValid=e},expression:"isFormValid"}},[a("v-simple-table",{attrs:{dense:""},scopedSlots:t._u([{key:"top",fn:function(){return[a("v-toolbar",{attrs:{flat:""}},[a("v-toolbar-title",[t._v("Edit company")])],1)]},proxy:!0}],null,!1,3437011982)},[a("tbody",[a("tr",[a("th",[t._v("Property")]),a("th",[t._v("Value")])]),a("tr",[a("td",[t._v("Id")]),a("td",[t._v(t._s(t.company.id))])]),a("tr",[a("td",[t._v("Name")]),a("td",[a("v-text-field",{attrs:{rules:[t.ruleRequired,t.ruleName]},model:{value:t.company.name,callback:function(e){t.$set(t.company,"name",e)},expression:"company.name"}})],1)])])])],1),a("v-row",{staticClass:"pa-5",attrs:{justify:"space-between"}},[a("v-btn",{on:{click:t.cancel}},[t._v("Cancel")]),a("v-btn",{staticClass:"error",on:{click:t.deleteCompany}},[t._v("Delete")]),a("v-btn",{staticClass:"success",attrs:{disabled:!t.isFormValid},on:{click:t.save}},[t._v("Save")])],1)],1)},r=[],s=a("1da1"),i=(a("96cf"),a("ac1f"),a("00b4"),a("d3b7"),a("e9c4"),{name:"EditCompany",data:function(){return{company:Object,isFormValid:Boolean,ruleRequired:function(t){return!!t||"Required"},ruleName:function(t){return/^[a-zA-ZæøåÆØÅ .'-]{2,20}$/.test(t)||"Name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters"}}},methods:{cancel:function(){this.$router.push("/admin")},deleteCompany:function(){var t=Object(s["a"])(regeneratorRuntime.mark((function t(){var e;return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.next=2,fetch("/api/companies/".concat(this.$route.params.id),{method:"DELETE"});case 2:if(e=t.sent,!e.ok){t.next=7;break}this.$router.push("/admin"),t.next=13;break;case 7:return t.t0=alert,t.next=10,e.text();case 10:t.t1=t.sent,(0,t.t0)(t.t1),"Unauthorized"==e.text&&this.$router.push("/login");case 13:case"end":return t.stop()}}),t,this)})));function e(){return t.apply(this,arguments)}return e}(),save:function(){var t=Object(s["a"])(regeneratorRuntime.mark((function t(){var e;return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:if(this.$refs.form.validate(),!this.isFormValid){t.next=15;break}return t.next=4,fetch("/api/companies/".concat(this.$route.params.id),{method:"PUT",headers:{"Content-Type":"application/json"},body:JSON.stringify(this.company)});case 4:if(e=t.sent,!e.ok){t.next=9;break}this.$router.push("/admin"),t.next=15;break;case 9:return t.t0=alert,t.next=12,e.text();case 12:t.t1=t.sent,(0,t.t0)(t.t1),"Unauthorized"==e.text&&this.$router.push("/login");case 15:case"end":return t.stop()}}),t,this)})));function e(){return t.apply(this,arguments)}return e}()},mounted:function(){var t=this;fetch("/api/companies/".concat(this.$route.params.id)).then((function(t){if(!t.ok)throw new Error;return t.json()})).then((function(e){t.company=e,t.isLoading=!1})).catch((function(){return t.$router.push("/login")}))}}),o=i,c=a("2877"),l=a("6544"),u=a.n(l),d=a("8336"),h=a("a523"),p=a("4bd4"),m=a("0fd9"),f=a("1f4f"),b=a("8654"),v=a("71d9"),x=a("2a7f"),y=Object(c["a"])(o,n,r,!1,null,null,null);e["default"]=y.exports;u()(y,{VBtn:d["a"],VContainer:h["a"],VForm:p["a"],VRow:m["a"],VSimpleTable:f["a"],VTextField:b["a"],VToolbar:v["a"],VToolbarTitle:x["a"]})},"8b37":function(t,e,a){}}]);
//# sourceMappingURL=chunk-3b980b4b.037ebe65.js.map