<script setup lang="ts">
</script>

<template>

  <body class="d-flex h-100 text-center text-white bg-dark">
    <div class="container d-flex w-100 h-100 p-3 mx-auto flex-column">
      <div class="cover-container d-flex w-100 h-100 p-3 mx-auto flex-column">
        <header class="mb-auto">
          <div>
            <h3 class="float-md-start mb-0">Horse Race</h3>
            <nav class="nav nav-masthead justify-content-center float-md-end">
              <RouterLink to="/" class="nav-link">Home</RouterLink>
              <RouterLink to="/race" class="nav-link">Create race</RouterLink>
              <RouterLink to="/horse" class="nav-link">Add Horse</RouterLink>

            </nav>
          </div>
        </header>
      </div>
      <RouterView />

      <footer class="mt-auto text-white-50">
        <p>
          Horse Race App
          by <a href="https://github.com/kpoldr" class="text-white">@kiwi</a>.
        </p>
      </footer>
    </div>
  </body>
</template>

<style>
@import "@/assets/base.css";

.bd-placeholder-img {
  font-size: 1.125rem;
  text-anchor: middle;
  -webkit-user-select: none;
  -moz-user-select: none;
  user-select: none;
}

@media (min-width: 768px) {
  .bd-placeholder-img-lg {
    font-size: 3.5rem;
  }
}
</style>
