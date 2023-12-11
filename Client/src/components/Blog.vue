<template>
  <v-btn @click="getPostsFromHub">get posts</v-btn>
  <v-btn @click="posts = []">clear</v-btn>
  <section id="log" class="container">
    <h2>Posts</h2>
    <p>{{content}}</p>
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
import * as signalR from "@microsoft/signalr";

export default {
  name: "Blog",
  setup() {
    let content = ref("Content");
    let posts = ref([]);
    let connection = ref(null);
    const state = reactive({
      appStatue: "Init",
      finalState: "End",
    });

    onMounted(() => {
      console.log(`try to  connect hub :  http://localhost:5185/postHub`);
      let hubAddr = "http://localhost:5185/postHub";
      connection.value = new signalR.HubConnectionBuilder()
        .withUrl(hubAddr) // Update with your server URL
        .build();
      console.log(`Connect Hub  ${hubAddr}`);
      connection.value.on("ReceivePosts", (message) => {
        console.log(`messages arrived from hub ${message}`);
        posts.value = message;
      });

      connection.value.start().catch((err) => console.error(err.toString()));
    });

    function getPostsFromHub() {
      connection.value
        .invoke("GetLatestPosts")
        .catch((err) => console.error(err.toString()));
      console.log(`request posts from hub`);
    }

    //http://localhost:5185/api/Blog/GetPosts
    //https://jsonplaceholder.typicode.com/posts
    function getPosts() {
      axios
        .get("http://localhost:5185/api/Blog/GetPosts")
        .then((response) => {
          console.log(`getPosts response : ${response.data}`);
          //content.value = response.data;
          posts.value = response.data;
        })
        .catch((error) => {
          console.log(error);
        });
    }
    return {
      content,
      state,
      getPosts,
      posts,
      getPostsFromHub,
      connection,
    };
  },
};
</script>

<style scoped></style>
