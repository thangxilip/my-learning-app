<script setup lang="ts">
import { ref } from 'vue';
import { FSRS, Rating, State, type Card, type ReviewLog } from '@squeakyrobot/fsrs';
import { Button } from '@/components/ui/button';

const fsrs = new FSRS();

const front = ref('');
const back = ref('');
const hasCard = ref(false);
const showAnswer = ref(false);

const card = ref<Card>(fsrs.createEmptyCard(new Date()));
const lastLog = ref<ReviewLog | null>(null);
const hasReviewed = ref(false);

const stateNames: Record<number, string> = {
  [State.New]: 'New',
  [State.Learning]: 'Learning',
  [State.Review]: 'Review',
  [State.Relearning]: 'Relearning',
};

const ratingNames: Record<number, string> = {
  [Rating.Again]: 'Again',
  [Rating.Hard]: 'Hard',
  [Rating.Good]: 'Good',
  [Rating.Easy]: 'Easy',
};

function formatDate(d: Date | null): string {
  if (!d) return '—';
  return d.toLocaleString();
}

function createCard() {
  if (!front.value.trim() || !back.value.trim()) return;
  card.value = fsrs.createEmptyCard(new Date());
  lastLog.value = null;
  hasReviewed.value = false;
  showAnswer.value = false;
  hasCard.value = true;
}

function review(grade: (typeof Rating)[keyof typeof Rating]) {
  const now = new Date();
  const result = fsrs.scheduleWithGrade(card.value, grade, now);
  card.value = result.card;
  lastLog.value = result.log;
  hasReviewed.value = true;
}
</script>

<template>
  <div class="app">
    <h1>FSRS practice</h1>

    <section class="panel">
      <h2>1. Create a card</h2>
      <label>
        Front
        <input v-model="front" type="text" placeholder="Question" />
      </label>
      <label>
        Back
        <input v-model="back" type="text" placeholder="Answer" />
      </label>
      <Button @click="createCard">Create card</Button>
    </section>

    <section v-if="hasCard" class="panel">
      <h2>2. Review</h2>
      <p class="face"><strong>Front:</strong> {{ front }}</p>
      <button v-if="!showAnswer" type="button" @click="showAnswer = true">Show answer</button>
      <template v-else>
        <p class="face"><strong>Back:</strong> {{ back }}</p>
        <p class="hint">How well did you recall it?</p>
        <div class="grades">
          <button type="button" @click="review(Rating.Again)">{{ ratingNames[Rating.Again] }}</button>
          <button type="button" @click="review(Rating.Hard)">{{ ratingNames[Rating.Hard] }}</button>
          <button type="button" @click="review(Rating.Good)">{{ ratingNames[Rating.Good] }}</button>
          <button type="button" @click="review(Rating.Easy)">{{ ratingNames[Rating.Easy] }}</button>
        </div>
      </template>
    </section>

    <section v-if="hasReviewed" class="panel result">
      <h2>3. Result (FSRS card state)</h2>
      <p><strong>State:</strong> {{ stateNames[card.state] ?? card.state }}</p>
      <p><strong>Stability:</strong> {{ card.stability.toFixed(2) }} days</p>
      <p><strong>Difficulty:</strong> {{ card.difficulty.toFixed(2) }}</p>
      <p><strong>Scheduled interval:</strong> {{ card.scheduled_days.toFixed(2) }} days</p>
      <p><strong>Reps:</strong> {{ card.reps }} · <strong>Lapses:</strong> {{ card.lapses }}</p>
      <p><strong>Next due:</strong> {{ formatDate(card.due) }}</p>
      <p><strong>Last review:</strong> {{ formatDate(card.last_review) }}</p>

      <h3 v-if="lastLog">Last review log</h3>
      <ul v-if="lastLog" class="log">
        <li>Rating: {{ ratingNames[lastLog.rating] ?? lastLog.rating }}</li>
        <li>State (before): {{ stateNames[lastLog.state] ?? lastLog.state }}</li>
        <li>Scheduled days: {{ lastLog.scheduled_days.toFixed(2) }}</li>
        <li>Review at: {{ formatDate(lastLog.review) }}</li>
      </ul>
    </section>
  </div>
</template>

<style scoped>
.app {
  max-width: 32rem;
  margin: 0 auto;
  padding: 1.5rem;
  font-family: system-ui, sans-serif;
  line-height: 1.5;
}

h1 {
  font-size: 1.25rem;
  margin-bottom: 1rem;
}

h2 {
  font-size: 1rem;
  margin: 0 0 0.75rem;
}

h3 {
  font-size: 0.9rem;
  margin: 1rem 0 0.5rem;
}

.panel {
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1rem;
}

.result {
  background: #f7f7f7;
}

label {
  display: block;
  margin-bottom: 0.75rem;
  font-size: 0.875rem;
}

input {
  display: block;
  width: 100%;
  margin-top: 0.25rem;
  padding: 0.4rem 0.5rem;
  box-sizing: border-box;
}

button {
  margin-top: 0.5rem;
  margin-right: 0.5rem;
  padding: 0.35rem 0.75rem;
  cursor: pointer;
}

.face {
  margin: 0.5rem 0;
}

.hint {
  font-size: 0.875rem;
  color: #444;
  margin: 0.75rem 0 0.5rem;
}

.grades {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.log {
  margin: 0;
  padding-left: 1.25rem;
  font-size: 0.875rem;
}
</style>
