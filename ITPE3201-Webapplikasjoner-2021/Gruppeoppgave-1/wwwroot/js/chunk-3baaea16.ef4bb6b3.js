(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-3baaea16"],{"1f4f":function(e,t,a){"use strict";var n=a("5530"),r=(a("a9e3"),a("8b37"),a("80d2")),i=a("7560"),s=a("58df");t["a"]=Object(s["a"])(i["a"]).extend({name:"v-simple-table",props:{dense:Boolean,fixedHeader:Boolean,height:[Number,String]},computed:{classes:function(){return Object(n["a"])({"v-data-table--dense":this.dense,"v-data-table--fixed-height":!!this.height&&!this.fixedHeader,"v-data-table--fixed-header":this.fixedHeader,"v-data-table--has-top":!!this.$slots.top,"v-data-table--has-bottom":!!this.$slots.bottom},this.themeClasses)}},methods:{genWrapper:function(){return this.$slots.wrapper||this.$createElement("div",{staticClass:"v-data-table__wrapper",style:{height:Object(r["e"])(this.height)}},[this.$createElement("table",this.$slots.default)])}},render:function(e){return e("div",{staticClass:"v-data-table",class:this.classes},[this.$slots.top,this.genWrapper(),this.$slots.bottom])}})},"2a7f":function(e,t,a){"use strict";a.d(t,"a",(function(){return i}));var n=a("71d9"),r=a("80d2"),i=Object(r["g"])("v-toolbar__title"),s=Object(r["g"])("v-toolbar__items");n["a"]},"4b8a":function(e,t,a){"use strict";a.r(t);var n=function(){var e=this,t=e.$createElement,a=e._self._c||t;return e.isLoading?e._e():a("v-container",{staticClass:"my-5",staticStyle:{"max-width":"32rem"}},[a("v-form",{ref:"form",model:{value:e.isFormValid,callback:function(t){e.isFormValid=t},expression:"isFormValid"}},[a("v-simple-table",{attrs:{dense:""},scopedSlots:e._u([{key:"top",fn:function(){return[a("v-toolbar",{attrs:{flat:""}},[a("v-toolbar-title",[e._v("Create new route")])],1)]},proxy:!0}],null,!1,1800119092)},[a("tbody",[a("tr",[a("th",[e._v("Property")]),a("th",[e._v("Value")])]),a("tr",[a("td",[e._v("Origin port")]),a("td",[a("v-select",{attrs:{items:e.ports,"item-text":"name","item-value":"id",rules:[e.ruleRequired,e.ruleUnique]},model:{value:e.route.origin.id,callback:function(t){e.$set(e.route.origin,"id",t)},expression:"route.origin.id"}})],1)]),a("tr",[a("td",[e._v("Destination port")]),a("td",[a("v-select",{attrs:{items:e.ports,"item-text":"name","item-value":"id",rules:[e.ruleRequired,e.ruleUnique]},model:{value:e.route.destination.id,callback:function(t){e.$set(e.route.destination,"id",t)},expression:"route.destination.id"}})],1)]),a("tr",[a("td",[e._v("Company")]),a("td",[a("v-select",{attrs:{items:e.companies,"item-text":"name","item-value":"id",rules:[e.ruleRequired]},model:{value:e.route.company.id,callback:function(t){e.$set(e.route.company,"id",t)},expression:"route.company.id"}})],1)])])])],1),a("v-row",{staticClass:"pa-5",attrs:{justify:"space-between"}},[a("v-btn",{staticClass:"error",on:{click:e.cancel}},[e._v("Cancel")]),a("v-btn",{staticClass:"success",attrs:{disabled:!e.isFormValid},on:{click:e.save}},[e._v("Save")])],1)],1)},r=[],i=a("1da1"),s=(a("96cf"),a("d3b7"),a("e9c4"),{name:"CreateRoute",data:function(){return{route:{origin:{id:null},destination:{id:null},company:{id:null}},companies:Array,ports:Array,isFormValid:Boolean,isLoading:!0,ruleRequired:function(e){return!!e||"Required"}}},computed:{ruleUnique:function(){var e=this;return function(){return e.route.origin.id!=e.route.destination.id||"Origin and destination cannot be the same"}}},watch:{route:{handler:function(){this.$refs.form.validate()},deep:!0}},methods:{cancel:function(){this.$router.push("/admin")},save:function(){var e=Object(i["a"])(regeneratorRuntime.mark((function e(){var t,a;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(this.$refs.form.validate(),!this.isFormValid){e.next=14;break}return e.next=4,fetch("/api/routes",{method:"POST",headers:{"Content-Type":"application/json"},body:JSON.stringify(this.route)});case 4:if(t=e.sent,!t.ok){e.next=9;break}this.$router.push("/admin"),e.next=14;break;case 9:return e.next=11,t.text();case 11:a=e.sent,alert(a),"Unauthorized"==a&&this.$router.push("/login");case 14:case"end":return e.stop()}}),e,this)})));function t(){return e.apply(this,arguments)}return t}()},mounted:function(){var e=this;return Object(i["a"])(regeneratorRuntime.mark((function t(){var a,n;return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.next=2,fetch("/api/ports");case 2:return a=t.sent,t.next=5,a.json();case 5:return e.ports=t.sent,t.next=8,fetch("/api/companies");case 8:return n=t.sent,t.next=11,n.json();case 11:e.companies=t.sent,e.isLoading=!1;case 13:case"end":return t.stop()}}),t)})))()}}),o=s,u=a("2877"),l=a("6544"),c=a.n(l),d=a("8336"),m=a("a523"),p=a("4bd4"),h=a("0fd9"),f=a("b974"),v=a("1f4f"),b=a("71d9"),x=a("2a7f"),g=Object(u["a"])(o,n,r,!1,null,null,null);t["default"]=g.exports;c()(g,{VBtn:d["a"],VContainer:m["a"],VForm:p["a"],VRow:h["a"],VSelect:f["a"],VSimpleTable:v["a"],VToolbar:b["a"],VToolbarTitle:x["a"]})},"8b37":function(e,t,a){}}]);
//# sourceMappingURL=chunk-3baaea16.ef4bb6b3.js.map