<script lang="ts">
import useVuelidate from "@vuelidate/core";
import { required, minLength, helpers, numeric, minValue, } from "@vuelidate/validators";

const timeValidation = helpers.regex(/^([0-1][0-9]|2[0-3]):[0-5][0-9]/gm);
const dateValidation = helpers.regex(
  /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/
);

export default {
  setup() {
    return {
      v$: useVuelidate(),
    };
  },

  data() {
    return {
      location: "",
      date: "",
      time: "",
      betAmount: 0,
      horses: [],
      horsesInRace: [],
      betFocus: 0,
    };
  },

  methods: {

    toBet(index: number) {
      console.log("clicked the ting");
      this.betFocus = index;
    },

    async addHorseToRace(index: number) {
      let a = this.horses.splice(index, 1)[0];
      // let a = this.horses[index];
      this.horsesInRace.push(a);
      this.horses.sort((a, b) => a.id - b.id);
    },

    async removeHorse(index: number) {
      let a = this.horsesInRace.splice(index, 1)[0];
      this.horses.push(a);
      this.horses.sort((a, b) => a.id - b.id);
    },

    changeColor(color: string) {
      document.getElementById("horsecolor")!.style.background = color;
    },
    async submitForm() {
      this.success = false;

      const isFormCorrect = await this.v$.$validate();

      if (isFormCorrect) {
        let id = await this.sendToDatabase();
        console.log(id)
        if (id) {
           this.$router.push("/race/" + id['id']);
        }
        // this.$router.push("/race/" + id);

      } else {
      }
    },

    async sendToDatabase() {
      const requestOptions = {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          location: this.location,
          date: this.date,
          time: this.time,
          horsesInRace: this.horsesInRace,
          selectedHorse: this.horsesInRace[this.betFocus],
          bet: this.betAmount,
        }),
      };

      let id;

      await fetch("http://localhost:9000/api/race", requestOptions)
        .then((response) => response.json())
        .then((data) => (id = data));
      
      
      return id;
    
    },
  },

  validations() {
    return {
      location: { required, minLength: minLength(1), $lazy: true },
      date: { required, minLength: minLength(1), dateValidation, $lazy: true },
      time: { required, minLength: minLength(1), timeValidation, $lazy: true },
      betAmount: { required, numeric, minValue: minValue(0.1), $lazy: true },
      horsesInRace: { required, minLength: minLength(1), $lazy: true },
    };
  },

  async beforeCreate() {
    await fetch("http://localhost:9000/api/horse")
      .then((response) => response.json())
      .then((data) => (this.horses = data));
  },

  created(): void {
    this.horses = [];
    this.horsesInRace = [];
    this.betFocus = 0;
  },
  beforeMount(): void {
    console.log("beforeMount");
  },
  mounted(): void {
    console.log("mounted");
  },
  beforeUpdate(): void {
    console.log("beforeUpdate");
  },
  // async updated(): void {

  // },
  activated(): void {
    console.log("activated");
  },
  deactivated(): void {
    console.log("deactivated");
  },
  beforeUnmount(): void {
    console.log("beforeUnmount");
  },
  unmounted(): void {
    console.log("unmounted");
  },
};
</script>

<template>
  <div class="container d-flex w-100 h-100 p-3 mx-auto flex-column">
    <div class="row">
      <div class="col-md-1"></div>
      <div class="col-md-10">
        <div class="horseInsert d-flex align-items-center">
          <div class="col-md-6">
            <div>
              <div class="form-group pb-3">
                <label class="d-flex p-1">Location:</label>

                <input v-model="location" class="form-control" placeholder="See tee 2-16" />

                <div class="text-danger text-start pt-2"  v-if="v$.location.$error" >
                  <small>*Must insert a location</small>
                </div>
              </div>
              <div class="row">
                <div class="col-md-6">
                  <label class="d-flex p-1">Date:</label>

                  <input
                    v-model="date"
                    class="form-control"
                    placeholder="12/02/2022"
                  />
                  <div
                    class="text-danger text-start pt-2"
                    v-if="v$.date.$error"
                  >
                    <small>*Incorrect date</small>
                  </div>
                </div>

                <div class="col-md-6">
                  <label class="d-flex p-1">Time:</label>

                  <input
                    v-model="time"
                    class="form-control"
                    placeholder="14:30"
                  />

                  <div
                    class="text-danger text-start pt-2"
                    v-if="v$.time.$error"
                  >
                    <small>*Incorrect time</small>
                  </div>
                </div>
              </div>
              <div class="form-group p-3">
                <div class="col pt-3">
                  <button @click="submitForm" class="btn btn-primary">
                    Create Race
                  </button>

                  <div
                    class="text-danger text-start pt-2"
                    v-if="v$.horsesInRace.$error"
                  >
                    <small>*No horses in race</small>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="col-md-1"></div>
          <div class="horsesInDiv col-md-5">
            <h5>Add a horse to the race</h5>
            <div class="rounded overflow overflow-auto">
              <ul class="list-group">
                <template v-for="(horse, index) in horses" :key="horse">
                  <li class="list-group-item">
                    <div class="row">
                      <div class="col-md-5 my-auto text-right">
                        <h5>
                          {{ horse.name }}
                        </h5>
                      </div>
                      <div class="col-md-3">
                        <img
                          class="img-fluid rounded"
                          src="@/assets/horse.png"
                          v-bind:style="{ 'background-color': horse.color }"
                          style="height: 4vh"
                        />
                      </div>
                      <div class="col-md-4 my-auto text-center">
                        <button
                          @click="addHorseToRace(index)"
                          class="btn btn-success"
                        >
                          Add
                        </button>
                      </div>
                    </div>
                  </li>
                </template>
              </ul>
            </div>
          </div>
        </div>
        <h4 class="pt-4">Choose a horse to bet on:</h4>
        <div class="horsesInRace d-flex mb-2">
          <div class="rounded overflow overflow-auto">
            <ul class="list-group">
              <template v-for="(horse, index) in horsesInRace" :key="horse">
                <li
                  @click="toBet(index)"
                  class="list-group-item"
                  v-if="betFocus == index"
                  :class="'border border-primary bg-light'"
                >
                  <div class="row">
                    <div class="col-md-5 my-auto text-right">
                      <small class="p-2">Bet amount: </small>
                      <input class="p-1" v-model="betAmount" />
                      <div
                        class="text-danger text-center pt-2"
                        v-if="v$.betAmount.$error"
                      >
                        <small>*Incorrect amount</small>
                      </div>
                    </div>

                    <div class="col-md-2 my-auto text-right">
                      <h5>
                        {{ horse.name }}
                      </h5>
                    </div>
                    <div class="col-md-3">
                      <img
                        class="img-fluid rounded"
                        src="@/assets/horse.png"
                        v-bind:style="{ 'background-color': horse.color }"
                        style="height: 8vh"
                      />
                    </div>
                    <div class="col-md-2 my-auto text-center">
                      <button
                        @click="removeHorse(index)"
                        class="btn btn-danger"
                      >
                        X
                      </button>
                    </div>
                  </div>
                </li>
                <li @click="toBet(index)" class="list-group-item" v-else>
                  <div class="row">
                    <div class="col-md-5 my-auto text-right">
                      
                    </div>
                    <div class="col-md-2 my-auto text-right">
                      <h5>
                        {{ horse.name }}
                      </h5>
                    </div>
                    <div class="col-md-3">
                      <img
                        class="img-fluid rounded"
                        src="@/assets/horse.png"
                        v-bind:style="{ 'background-color': horse.color }"
                        style="height: 8vh"
                      />
                    </div>
                    <div class="col-md-2 my-auto text-center">
                      <button
                        @click="removeHorse(index)"
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
    min-height: 45vh;
    flex-grow: 1;
  }
  .horsesInDiv .overflow {
    height: 30vh;
    flex-grow: 0.4;
  }
  .horsesInRace {
    min-height: 50vh;
    max-height: 50vh;
    flex-grow: 1;
  }
  .horsesInRace .overflow {
    overflow: auto;
    height: 45vh;
    flex-grow: 1;
    align-items: stretch;
    overflow-x: scroll;
    background: white;
  }
}
</style>
