<script lang="ts">
export default {

  data() {
    return {
      races: [],
    };
  },

  methods: {

    getFormattedDate(dateString: string) {
      let date = new Date(dateString);
      var year = date.getFullYear();

      var month = (1 + date.getMonth()).toString();
      month = month.length > 1 ? month : "0" + month;

      var day = date.getDate().toString();
      day = day.length > 1 ? day : "0" + day;

      return day + "/" + month + "/" + year;
    },

    compareDates(dateString: string) {
      let raceDate = new Date(dateString);
      let currentDate = new Date();

      if (raceDate.getTime() > currentDate.getTime()) {
        return false;
      }

      return true;
    },

    getFormattedTime(dateString: string) {
      let date = new Date(dateString);

      var hours = date.getHours().toString();
      hours = hours.length > 1 ? hours : "0" + hours;

      var minutes = date.getMinutes().toString();
      minutes = minutes.length > 1 ? minutes : "0" + minutes;

      return hours + ":" + minutes;
    },
  },

  async beforeCreate() {
    console.log(
      await fetch("http://localhost:9000/api/race")
        .then((response) => response.json())
        .then((data) => (this.races = data))
    );
  },
};
</script>

<template>
  <div class="container w-100 vh-100 p-3 mx-auto flex-column">
    <h1 class="p-4">Welcome to the horse race app</h1>
    <h4 class="p-2">Race list</h4>
    <div class="row">
      <div class="col-md-1"></div>
      <div class="col-md-10">
        <div class="racesDiv d-flex mb-2">
          <div class="rounded overflow overflow-auto">
            <ul class="races list-group">
              <template v-for="race in races" :key="race">
                <li
                  @click="$router.push('/race/' + race.id)"
                  class="list-group-item"
                >
                  <div class="row">
                    <div class="col-md-4">
                      <div class="col">
                        <h5>Location:</h5>
                      </div>
                      <div class="col">
                        {{ race.location }}
                      </div>
                    </div>

                    <div class="col-md-2">
                      <div class="row">
                        <div class="col">
                          <h5>Date:</h5>
                        </div>
                        <div class="col">
                          {{ getFormattedDate(race.datetime) }}
                        </div>
                      </div>
                    </div>

                    <div class="col-md-3">
                      
                        <div class="col">
                          <h5>Time:</h5>
                        </div>
                        <div class="col">
                          {{ getFormattedTime(race.datetime) }}
                        </div>
                      
                    </div>

                    <div class="col-md-3 my-auto text-center pe-5">
                      <h4 
                        v-if="compareDates(race.datetime)"
                        style="color: green"
                      >
                        Finished
                      </h4>
                      <h4 v-else style="color: red">Not started</h4>
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
  .racesDiv {
    min-height: 50vh;
    flex-grow: 1;
  }
  .overflow {
    overflow: auto;
    height: 70vh;
    flex-grow: 1;
    align-items: stretch;
    overflow-x: scroll;
    background: white;
  }
  .races li :hover {
    background-color: rgb(231, 231, 231);
    cursor: pointer;
  }
}
</style>
