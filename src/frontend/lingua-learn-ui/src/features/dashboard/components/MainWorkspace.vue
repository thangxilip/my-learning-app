<script setup lang="ts">
import { Layers } from 'lucide-vue-next'
import { ref } from 'vue'

import { Button } from '@/components/ui/button'
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from '@/components/ui/tabs'
import BoardsPlaceholder from '@/features/boards/components/BoardsPlaceholder.vue'
import FlashcardFilterBar from '@/features/flashcards/components/FlashcardFilterBar.vue'
import FlashcardsDeckGrid from '@/features/flashcards/components/FlashcardsDeckGrid.vue'
import { useFlashcards } from '@/composables/useFlashcards'

const mainTab = ref('flashcards')

const {
  statusFilter,
  summaryLine,
  filteredDecks,
  draftCardText,
  setStatusFilter,
  setDraftForDeck,
  addCardToDeck,
  addDeck,
  deckStats,
} = useFlashcards()

function onStudyDeck() {
  /* Study session / FSRS integration */
}
</script>

<template>
  <Tabs v-model="mainTab" class="gap-0">
    <header
      class="mb-8 flex flex-col gap-6 lg:mb-10 lg:grid lg:grid-cols-[minmax(0,1fr)_auto_minmax(0,1fr)] lg:items-center lg:gap-4"
    >
      <div class="min-w-0 space-y-1 justify-self-start">
        <template v-if="mainTab === 'flashcards'">
          <h1 class="text-3xl font-bold tracking-tight text-foreground">
            Flashcards
          </h1>
          <p class="text-sm text-muted-foreground">
            {{ summaryLine }}
          </p>
        </template>
        <template v-else>
          <h1 class="text-3xl font-bold tracking-tight text-foreground">
            Boards
          </h1>
          <p class="text-sm text-muted-foreground">
            Organize todos alongside your decks
          </p>
        </template>
      </div>

      <TabsList
        class="h-10 w-full max-w-md justify-self-center lg:w-auto lg:min-w-[220px]"
      >
        <TabsTrigger value="flashcards" class="flex-1 px-4">
          Flashcards
        </TabsTrigger>
        <TabsTrigger value="boards" class="flex-1 px-4">
          Boards
        </TabsTrigger>
      </TabsList>

      <div
        class="flex flex-col items-stretch gap-3 sm:flex-row sm:items-center sm:justify-end justify-self-end lg:min-w-0"
      >
        <FlashcardFilterBar
          v-if="mainTab === 'flashcards'"
          :model-value="statusFilter"
          class="sm:justify-end"
          @update:model-value="setStatusFilter"
        />
        <Button
          v-if="mainTab === 'flashcards'"
          type="button"
          class="shrink-0 gap-2"
          @click="addDeck"
        >
          <Layers class="size-4" />
          New deck
        </Button>
      </div>
    </header>

    <TabsContent value="flashcards" class="mt-0 outline-none">
      <FlashcardsDeckGrid
        :decks="filteredDecks"
        :draft-by-deck-id="draftCardText"
        :deck-stats="deckStats"
        @update:draft="setDraftForDeck"
        @add-card="addCardToDeck"
        @study="onStudyDeck"
        @add-deck="addDeck"
      />
    </TabsContent>

    <TabsContent value="boards" class="mt-0 outline-none">
      <BoardsPlaceholder />
    </TabsContent>
  </Tabs>
</template>
