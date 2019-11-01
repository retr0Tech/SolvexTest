<template>
  <div class="home-page">
    <div class="banner">
      <div class="container">
        <h1 class="logo-font">Clients</h1>
      </div>
    </div>
    <div align="center">
      <input
        v-model="client.name"
        @keyup.enter="addClient"
        type="text"
        class="form-control"
        placeholder="Name to add..."
      />
      <br />
      <button v-on:click="addClient" type="button" class="btn btn-success">Add</button>
    </div>
    <div class="container page">
      <div class="row">
        <div class="col-md-9">
          <div class="feed-toggle">
            <ul class="nav nav-pills outline-active">
              <li class="nav-item">
                <span class="nav-link active">Clients</span>
              </li>
            </ul>
          </div>
          <div
            class="article-preview"
            v-for="(client, index) in clients"
            :key="client.id"
            :client="client"
          >
            <div class="article-meta">
              <img src="https://icon-library.net/images/client-icon/client-icon-0.jpg" />
              <div class="info">
                <span v-if="show && idUpdate == index">
                  <!-- Formulario para actualizar -->
                  <input v-model="nameUpdate" type="text" class="form-control" />
                </span>
                <span v-else>
                  <span class="author">{{ client.name }}</span>
                </span>
                <span class="date">Client</span>
              </div>
              <div class="float-right right">
                <span v-if="show && idUpdate == index">
                  <button class="btn btn-success" v-on:click="editData(client, client.id, index)">
                    <i class="ion-heart"></i> Save
                  </button> |
                  <button class="btn btn-danger" v-on:click="showNormal(index)">
                    <i class="ion-heart"></i> Cancel
                  </button>
                </span>
                <span v-else>
                  <button class="btn btn-warning" v-on:click="showNormal(index)">
                    <i class="ion-heart"></i> Edit
                  </button> |
                  <button class="btn btn-danger" v-on:click="deleteData(client, client.id)">
                    <i class="ion-heart"></i> Delete
                  </button>
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import axios from "axios";
import swal from "sweetalert";

export default {
  data() {
    return {
      clients: [],
      client: {
        name: ""
      },
      apiUrl: "https://localhost:5001/api/clients/",
      idUpdate: -1,
      nameUpdate: "",
      show: false
    };
  },
  created() {
    this.getClients();
  },
  methods: {
    getClients: function() {
      axios.get(this.apiUrl).then(response => (this.clients = response.data));
    },
    addClient() {
      var _this = this;
      if (this.client.name !== "") {
        axios
          .post(this.apiUrl, this.client)
          .then(function(response) {
            console.log(response.data);
            _this.getClients();
            swal(
              "Client Added!",
              "The Client was added to the server",
              "success"
            );
          })
          .catch(function(error) {
            swal(":(", "Something went wrong!", "error");
          });

        // Vaciamos el formulario de añadir
        this.clear();
      }
    },
    editData(client, id, index) {
      var _this = this;
      axios
        .put(this.apiUrl + client.id, {
          id: client.id,
          name: this.nameUpdate
        })
        .then(function(response) {
          _this.getClients();
          swal("Client Updated!", "The Client was updated", "success");
        })
        .catch(function(error) {
          swal(":(", "Something went wrong");
        });
      this.show = !this.show;
      this.idUpdate = index;
    },
    deleteData: function(client, id) {
      var _this = this;
      swal({
        title: "Are you sure you want to delete it?",
        text: "You will not be able to recover this info!",
        icon: "warning",
        buttons: true,
        dangerMode: true
      }).then(willDelete => {
        if (willDelete) {
          axios.delete(this.apiUrl + client.id).then(response => {
            this.clients.splice(id, 1);
            _this.getClients();
          });
          swal("Your client has been deleted!", {
            icon: "success"
          }).catch(function(error) {
            swal(":(", "Something went wrong");
          });
        }
      });
    },
    clear: function() {
      this.client = {
        name: ""
      };
    },
    showNormal: function(index) {
      this.show = !this.show;
      this.idUpdate = index;
    }
  }
};
</script>