<template>
  <div id="app">
    <header>
      <nav>
        <img src="./assets/logo.png" style="height:30px; padding: 10px; float: left;">
        <router-link to="/">Home</router-link>
        <router-link to="/design">Design</router-link>
        <router-link to="/pack">Pack</router-link>
      </nav>
    </header>
    <main>
      <router-view/>
    </main>
  </div>
</template>

<style>
html,
body {
  margin: 0px;
  padding: 0px;
}

body {
  font-family: Calibri;
  color: #222;
  background: linear-gradient(to bottom, #444, #aaa);
  overflow-y: scroll;
  background-attachment: fixed;
}

#app {
  padding: 0px;
  margin: 0px;
}

header {
  position: fixed;
  height: 50px;
  top: 0px;
  width: 100%;
  z-index: 999;
  background: #222;
  color: white;
  padding: 0px;
  margin: 0px;
}

/* main after header starts 40px down */
header + main {
  margin-top: 50px;
}

main {
  margin: 0 auto;
  padding: 10px;
  background: white;
  width: 1024px;
  min-height: 400px;
  box-sizing: border-box;
}

nav {
  margin: 0 auto;
  width: 1024px;
  padding: 0px;
}

header nav a {
  color: #999;
  float: left;
  height: 50px;
  line-height: 50px;
  padding: 0px 16px;
  text-decoration: none;
  border-right: 1px solid #eee;
}

header a:hover {
  color: #fff;
  background: #666;
}

#nav a.router-link-exact-active {
  color: #42b983;
}

.logo {
  vertical-align: middle;
  height: 30px;
}

label {
  display: block;
  padding: 0px;
  margin-top: 0px;
  font-weight: 600;
  margin-top: 10px;
}

input, select {
  margin-right: 3px;
  border: 1px solid #222;
  height: 30px;
  min-height: 30px;
  width: 50%;
}

h1,h2,h3,h4,h5,h6 {
  margin: 4px 0px;
  padding: 0px;
}

button {
  box-sizing: border-box;
  display: inline-block;
  height: 30px;
  padding: 0 20px;
  text-align: center;
  text-decoration: none;
  white-space: nowrap;
  border-radius: 4px;
  cursor: pointer;
  box-sizing: border-box;
  color: #234567;
  background-color: rgb(153, 204, 255); /* #33C3F0; */
  border-color: #234567; /* #33C3F0; */
  margin-right: 2px;
}

button:hover {
  background-color: #aaa;
}

legend {
  height: 30px;
  line-height : 30px;
  margin-top: -15px;
  border: 1px solid #aaa;
  background-color: #222;
  color: #22aa22;
  z-index: 999;
  border-radius: 4px;
  padding: 0px 8px;
}

fieldset {
  margin: 0px;
  margin-top:3px;
  padding: 3px;
  border: 1px solid #aaa;
  border-width: 1px;
  border-radius: 4px;
}

div.buttonGroup {
  margin: 3px 0px;
}

input[type='checkbox'] {
  text-align: left;
  width: 20px;
}

textarea {
  width: 100%;
}

</style>
