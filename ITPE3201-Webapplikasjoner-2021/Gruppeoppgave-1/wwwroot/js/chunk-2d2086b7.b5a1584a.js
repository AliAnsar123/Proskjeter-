(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-2d2086b7"],{a55b:function(e,r,a){"use strict";a.r(r);var t=function(){var e=this,r=e.$createElement,a=e._self._c||r;return a("v-container",{staticClass:"my-5",staticStyle:{"max-width":"16rem"}},[a("v-form",{ref:"form",model:{value:e.valid,callback:function(r){e.valid=r},expression:"valid"}},[a("v-row",[a("v-col",{attrs:{cols:"12"}},[a("h1",[e._v("Admin login")]),a("v-text-field",{attrs:{rules:e.usernameRules,label:"User name",required:""},model:{value:e.username,callback:function(r){e.username=r},expression:"username"}})],1),a("v-col",{attrs:{cols:"12"}},[a("v-text-field",{attrs:{rules:e.passwordRules,label:"Password",required:"",type:"password"},model:{value:e.password,callback:function(r){e.password=r},expression:"password"}})],1),a("v-col",{attrs:{cols:"12"}},[a("p",{staticClass:"red--text"},[e._v(e._s(e.errortext))]),a("v-btn",{attrs:{color:"primary"},on:{click:e.login}},[e._v("Login")])],1)],1)],1)],1)},n=[],s=a("1da1"),o=(a("96cf"),a("ac1f"),a("00b4"),a("d3b7"),a("e9c4"),{name:"Login",data:function(){return{valid:null,username:null,password:null,errortext:null,usernameRules:[function(e){return!!e||"Username is required"},function(e){return/^[a-zA-ZæøåÆØÅ.-]{2,20}$/.test(e)||"Username must contain between 2 and 20 characters"}],passwordRules:[function(e){return!!e||"Password is required"},function(e){return/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/.test(e)||"Password must contain at least 6 characters, containing at least one letter and one number"}]}},methods:{login:function(){var e=this;return Object(s["a"])(regeneratorRuntime.mark((function r(){var a;return regeneratorRuntime.wrap((function(r){while(1)switch(r.prev=r.next){case 0:if(e.$refs.form.validate(),r.prev=1,!e.valid){r.next=11;break}return r.next=5,fetch("/api/login",{method:"POST",headers:{"Content-Type":"application/json"},body:JSON.stringify({Username:e.username,Password:e.password})});case 5:if(a=r.sent,a.ok){r.next=10;break}throw new Error;case 10:e.$router.push("/admin");case 11:r.next=16;break;case 13:r.prev=13,r.t0=r["catch"](1),e.errortext="Wrong username or password";case 16:case"end":return r.stop()}}),r,null,[[1,13]])})))()}}}),l=o,i=a("2877"),u=a("6544"),c=a.n(u),d=a("8336"),m=a("62ad"),p=a("a523"),f=a("4bd4"),v=a("0fd9"),w=a("8654"),b=Object(i["a"])(l,t,n,!1,null,null,null);r["default"]=b.exports;c()(b,{VBtn:d["a"],VCol:m["a"],VContainer:p["a"],VForm:f["a"],VRow:v["a"],VTextField:w["a"]})}}]);
//# sourceMappingURL=chunk-2d2086b7.b5a1584a.js.map