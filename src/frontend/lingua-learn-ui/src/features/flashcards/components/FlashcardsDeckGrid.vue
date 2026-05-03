<script setup lang="ts">
import AddDeckCard from '@/features/flashcards/components/AddDeckCard.vue'
import DeckCard from '@/features/flashcards/components/DeckCard.vue'
import type { FlashcardDeck } from '@/features/flashcards/types'

defineProps<{
  decks: FlashcardDeck[]
  draftByDeckId: Record<string, string>
  deckStats: (deck: FlashcardDeck) => { total: number; mastered: number; pct: number }
}>()

const emit = defineEmits<{
  (e: 'update:draft', deckId: string, value: string): void
  (e: 'add-card', deckId: string): void
  (e: 'study', deckId: string): void
  (e: 'add-deck'): void
}>()
</script>

<template>
  <div
    class="grid gap-4 sm:grid-cols-2 xl:grid-cols-3"
  >
    <DeckCard
      v-for="deck in decks"
      :key="deck.id"
      :deck="deck"
      :draft="draftByDeckId[deck.id] ?? ''"
      :stats="deckStats(deck)"
      @update:draft="emit('update:draft', deck.id, $event)"
      @add-card="emit('add-card', deck.id)"
      @study="emit('study', deck.id)"
    />
    <AddDeckCard @click="emit('add-deck')" />
  </div>
</template>
