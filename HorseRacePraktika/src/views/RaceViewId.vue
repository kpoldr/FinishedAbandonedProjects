<script lang="ts">
import useVuelidate from "@vuelidate/core";
import {
  required,
  minLength,
  helpers,
  numeric,
  minValue,
} from "@vuelidate/validators";

const timeValidation = helpers.regex(/^([0-1][0-9]|2[0-3]):[0-5][0-9]/gm);
const dateValidation = helpers.regex(
  /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/
);

export default {
  data() {
    return {
      bet: 0,
      race: [],
      raceId: 0,
    };
  },

  methods: {

    compareDates(dateString: string) {
      let raceDate = new Date(dateString);
      let currentDate = new Date();

      if (raceDate.getTime() > currentDate.getTime()) {
        return false;
      }

      return true;
    },
    GetRemaningDays(dateString: string) {

      let raceDate = new Date(dateString);
      let currentDate = new Date();

      var delta = Math.abs(raceDate.getTime() - currentDate.getTime()) / 1000;

      // calculate (and subtract) whole days
      var days = Math.floor(delta / 86400);
      delta -= days * 86400;

      // calculate (and subtract) whole hours
      var hours = Math.floor(delta / 3600) % 24;
      delta -= hours * 3600;

      // calculate (and subtract) whole minutes
      var minutes = Math.floor(delta / 60) % 60;
      delta -= minutes * 60;

      return `${days} days ${hours} hours ${minutes} minutes`;
    },
  },

  async beforeCreate() {
    await fetch("http://localhost:9000/api/race/" + this.$route.params.id)
      .then((response) => response.json())
      .then((data) => (this.race = data));

    if (this.race != null) {
      this.race.forEach((horse) => {
        if (horse["bet"] != null) {
          this.bet = horse["bet"];
        }
      });
    }
  },
};
</script>

<template>
  <div class="container w-100 vh-100 p-3 mx-auto flex-column">
    <div v-if="race.length > 0">
      <div class="row">
        <div v-if="compareDates(race[0].datetime)">
          <div class="col-md-1"></div>
          <div class="col-md-10 mx-auto">
            <div class="row">
              <div class="col text-start">
                <h4 class="p-1">Race Results</h4>
              </div>
              <div class="col text-end">
                <h4 class="p-2">
                  Bet result:
                  <span v-if="race[0].bet != null" class="p-2 text-success">
                    +{{ bet * race.length }}$
                  </span>
                  <span v-else class="p-2 text-danger"> -{{ bet }}$ </span>
                </h4>
              </div>
            </div>
            <div class="horsesThatRaced mb-2">
              <div class="rounded overflow overflow-auto">
                <ul class="list-group">
                  <template v-for="(horse, index) in race" :key="index">
                    <li
                      v-if="horse.bet > 0"
                      :class="'border border-success'"
                      class="list-group-item bg-light"
                    >
                      <div class="row">
                        <div class="col-md-5 my-auto text-right">
                          <h2 v-if="index == 0" style="color: #ffbb00">
                            {{ horse.place }}
                          </h2>
                          <h2 v-else-if="index == 1" style="color: #a1a09f">
                            {{ horse.place }}
                          </h2>
                          <h2 v-else-if="index == 2" style="color: #804000">
                            {{ horse.place }}
                          </h2>
                          <h2 v-else>{{ horse.place }}</h2>
                        </div>
                        <div class="col-md-2 my-auto text-right">
                          <h3>{{ horse.name }}</h3>
                        </div>
                        <div class="col-md-5">
                          <div class="row">
                            <div class="col-md-4"></div>

                            <div class="col-md-4">
                              <img
                                class="img-fluid rounded"
                                src="@/assets/horse.png"
                                v-bind:style="{
                                  'background-color': horse.color,
                                }"
                                style="height: 8vh"
                              />
                            </div>

                            <div class="col-md-4 my-auto">
                              <p
                                v-if="horse.place > 1"
                                style="color: red"
                                class="my-auto"
                              >
                                ${{ horse.bet }}
                              </p>
                              <p v-else style="color: green" class="my-auto">
                                ${{ horse.bet }}
                              </p>
                            </div>
                          </div>
                        </div>
                      </div>
                    </li>
                    <li v-else class="list-group-item bg-light">
                      <div class="row">
                        <div class="col-md-5 my-auto text-right">
                          <h2 v-if="index == 0" style="color: #ffbb00">
                            {{ horse.place }}
                          </h2>
                          <h2 v-else-if="index == 1" style="color: #a1a09f">
                            {{ horse.place }}
                          </h2>
                          <h2 v-else-if="index == 2" style="color: #804000">
                            {{ horse.place }}
                          </h2>
                          <h2 v-else>{{ horse.place }}</h2>
                        </div>
                        <div class="col-md-2 my-auto text-right">
                          <h3>{{ horse.name }}</h3>
                        </div>
                        <div class="col-md-5">
                          <div class="row">
                            <div class="col-md-4"></div>

                            <div class="col-md-4">
                              <img
                                class="img-fluid rounded"
                                src="@/assets/horse.png"
                                v-bind:style="{
                                  'background-color': horse.color,
                                }"
                                style="height: 8vh"
                              />
                            </div>

                            <div class="col-md-4 my-auto"></div>
                          </div>
                        </div>
                      </div>
                    </li>
                  </template>
                </ul>
              </div>
            </div>
            <div class="col my-top">
              <button @click="$router.push('/')" class="btn-lg btn-primary">
                Back
              </button>
            </div>
          </div>
          <div class="col-md-1"></div>
          
        </div>
        
        <div v-else class="vh-100">
          <h3 class="justify-content-center pt-5">This race starts in</h3>
          <h1 class="justify-content-center pt-5">{{GetRemaningDays(race[0].datetime)}}</h1>

        </div>
        
      </div>
    </div>
    <div v-else class="pt-5">
      <h1 class="mx-auto">Race not Found</h1>
    </div>
  </div>
</template>

<style>
@media (min-width: 1024px) {
  .horsesInDiv .overflow {
    height: 30vh;
    flex-grow: 0.4;
  }
  .horsesThatRaced {
    min-height: 66vh;
    flex-grow: 1;
  }
  .horsesThatRaced .overflow {
    overflow: auto;
    height: 65vh;
    flex-grow: 1;
    align-items: stretch;
    overflow-x: scroll;
    background: white;
  }
}
</style>
