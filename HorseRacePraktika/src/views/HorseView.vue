<script lang="ts">
import useValidate from "@vuelidate/core";
import { required, minLength } from "@vuelidate/validators";

export default {
  data() {
    return {
      v$: useValidate(),
      horseName: "",
      horseColor: "#000000",
      horses: [],
      horseNameErrors: [],
      success: false,
    };
  },

  methods: {
    async updateHorseList() {
      console.log(
        await fetch("http://localhost:9000/api/horse")
        .then((response) => response.json())
        .then((data) => (this.horses = data))
      )
      
    },

    async removeHorse(id: number) {
      let deleted = await fetch(`http://localhost:9000/api/horse/${id}`, {
        method: "delete",
      });

      if (deleted) {
        await this.updateHorseList();
      }
    },

    changeColor(color: string) {
      document.getElementById("horsecolor")!.style.background = color;
    },
    submitForm() {
      this.success = false;
      this.horseNameErrors = [];
      console.log(this.v$);
      this.v$.$validate();
      if (!this.v$.$error) {
        this.sendToDatabase(this.horseName, this.horseColor);
        this.horseName = "";
        this.success = true;
      } else {
        this.horseNameErrors.push("Name cannot be empty");
      }
    },

    async sendToDatabase() {
      const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name: this.horseName, color: this.horseColor }),
      };

      await fetch("http://localhost:9000/api/horse", requestOptions);

      this.updateHorseList();
    },
  },

  validations() {
    return {
      horseName: { required, minLength: minLength(1) },
    };
  },

  watch: {
    horseColor(): void {
      this.changeColor(this.horseColor);
    },
  },
  async beforeCreate() {
    await fetch("http://localhost:9000/api/horse")
      .then((response) => response.json())
      .then((data) => (this.horses = data));
  },
}
</script>

<template>
  <div class="container d-flex w-100 vh-100 p-3 mx-auto flex-column">
    <div class="row">
      <div class="col-md-1"></div>
      <div class="col-md-10">
        <div class="horseInsert d-flex  align-items-center">
          <div class="col-md-6">
            <div class="text-success" v-if="success">Horse added</div>
            
            
            <div>
              <div class="form-group pb-3">
                <label class="d-flex p-1">Name:</label>
                <input
                  v-model="horseName"
                  class="form-control"
                  placeholder="Enter name"
                  
                />
                <template v-for="error in horseNameErrors" :key="error">
                  <div class="text-danger text-start pt-2">
                    <small>*{{ error }}</small>
                  </div>
                </template>
                
              </div>
              <div class="form-group">
                <div class="col">
                  <label for="selectedcolor" class="d-flex p-1"
                    >Horses color:
                  </label>
                  <input
                    type="color"
                    id="selectcolor"
                    name="head"
                    style="width: 100%"
                    v-model="horseColor"
                    @change="changeColor()"
                  />
                </div>

                <div class="col pt-3">
                  <button @click="submitForm" class="btn btn-primary">
                    Submit
                  </button>
                </div>
              </div>
            </div>
          </div>

          <div class="col-md-1"></div>
          <div class="col-md-5">
            
              <img
                id="horsecolor"
                class="d-flex rounded mx-auto"
                src="@/assets/horse.png"
                style="background-color: black"
              />
            
          </div>
        </div>
        <h4>Horses</h4>
        <div class="horses d-flex mb-2">
          <div class="overflow overflow-auto">
            <ul class="list-group">
              <template v-for="horse in horses" :key="horse">
                <li class="list-group-item">
                  <div class="row">
                    <div class="col-md-6 my-auto text-right">
                      <h3>
                        {{ horse.name }}
                      </h3>
                    </div>
                    <div class="col-md-4">
                      <img
                        class="img-fluid rounded"
                        src="@/assets/horse.png"
                        v-bind:style="{ 'background-color': horse.color }"
                        style="height: 8vh"
                      />
                    </div>
                    <div class="col-md-2 my-auto">
                      <button
                        @click="removeHorse(horse.id)"
                        class="btn btn-danger"
                      >
                        X
                      </button>
                    </div>
                  </div>
                </li>
              </template>
            </ul>
          </div>
        </div>
      </div>
      <div class="col-md-1"></div>
    </div>
  </div>
</template>

<style>
@media (min-width: 1024px) {
  .horseInsert {
    min-height: 35vh;
    flex-grow: 1;
  }
  .horses {
    min-height: 50vh;
    max-height: 50vh;
    flex-grow: 1;
  }
  .horses .overflow {
    overflow: auto;
    height: 45vh;
    flex-grow: 1;
    align-items: stretch;
    overflow-x: scroll;
    background: white;
  }
}
</style>
