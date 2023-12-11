<template>
  <v-btn @click="getPosts">get posts</v-btn>
  <v-btn @click="posts = []">clear</v-btn>
  <section id="log" class="container">
    <h2>Posts</h2>
    <v-list lines="three">
      <v-list-item
        v-for="post in posts"
        :key="post.id"
        :title="post.title"
        :subtitle="post.body"
      ></v-list-item>
    </v-list>
  </section>
</template>

<script>
import { ref, reactive, toRefs, computed, watch, inject, onMounted } from "vue";
import axios from "axios";

export default {
  name: "Blog",
  setup() {
    let fname = ref("Amiel");
    let posts = ref([]);
    const state = reactive({
      appStatue: "Init",
      finalState: "End",
    });

    function getPosts() {
      axios
        .get("https://jsonplaceholder.typicode.com/posts")
        .then((response) => {
          console.log(response);
          posts.value = response.data;
        })
        .catch((error) => {
          console.log(error);
        });
    }
    return {
      fname,
      state,
      getPosts,
      posts,
    };
  },
};
</script>

<style scoped></style>
