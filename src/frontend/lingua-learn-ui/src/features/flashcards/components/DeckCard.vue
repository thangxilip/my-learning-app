<script setup lang="ts">
import { MoreHorizontal, Play, Plus } from 'lucide-vue-next'

import FlashcardStatusBadge from '@/features/flashcards/components/FlashcardStatusBadge.vue'
import type { FlashcardDeck } from '@/features/flashcards/types'
import { Button } from '@/components/ui/button'
import { Card } from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Progress } from '@/components/ui/progress'
import { cn } from '@/lib/utils'

const props = defineProps<{
  deck: FlashcardDeck
  draft: string
  stats: { total: number; mastered: number; pct: number }
}>()

const emit = defineEmits<{
  (e: 'update:draft', value: string): void
  (e: 'add-card'): void
  (e: 'study'): void
}>()
</script>

<template>
  <Card class="relative h-full gap-0 overflow-hidden py-0 shadow-sm">
    <div :class="cn('h-1 w-full', props.deck.accentClass)" aria-hidden="true" />

    <div class="flex flex-col gap-4 px-5 pb-5 pt-4">
      <div class="flex items-start justify-between gap-2">
        <div class="flex min-w-0 items-start gap-2">
          <span
            class="mt-1.5 size-2 shrink-0 rounded-full"
            :class="props.deck.accentClass"
            aria-hidden="true"
          />
          <div class="min-w-0 space-y-1">
            <div class="flex flex-wrap items-center gap-2">
              <h3 class="truncate text-base font-semibold tracking-tight">
                {{ props.deck.title }}
              </h3>
              <span
                class="inline-flex h-6 min-w-6 items-center justify-center rounded-full bg-muted px-2 text-xs font-medium text-muted-foreground"
              >
                {{ props.stats.total }}
              </span>
            </div>
            <p class="text-sm text-muted-foreground">
              {{ props.deck.description }}
            </p>
          </div>
        </div>
        <Button
          type="button"
          variant="ghost"
          size="icon"
          class="text-muted-foreground shrink-0"
          aria-label="Deck options"
        >
          <MoreHorizontal class="size-4" />
        </Button>
      </div>

      <div class="space-y-2">
        <div class="flex items-center justify-between text-xs text-muted-foreground">
          <span>
            {{ props.stats.mastered }}/{{ props.stats.total }} mastered
          </span>
          <span>{{ props.stats.pct }}%</span>
        </div>
        <Progress :model-value="props.stats.pct" class="h-1.5" />
      </div>

      <div class="flex gap-2">
        <div class="relative min-w-0 flex-1">
          <Plus
            class="pointer-events-none absolute left-2.5 top-1/2 size-4 -translate-y-1/2 text-muted-foreground"
            aria-hidden="true"
          />
          <Input
            :model-value="props.draft"
            placeholder="Add card"
            class="h-9 pl-9"
            @update:model-value="emit('update:draft', $event as string)"
            @keydown.enter.prevent="emit('add-card')"
          />
        </div>
        <Button
          type="button"
          class="shrink-0 gap-1.5 px-3"
          @click="emit('study')"
        >
          <Play class="size-4 fill-current" />
          Study
        </Button>
      </div>

      <ul
        v-if="props.deck.cards.length"
        class="divide-y divide-border rounded-lg border border-border/80"
      >
        <li
          v-for="card in props.deck.cards"
          :key="card.id"
          class="flex flex-wrap items-center justify-between gap-2 px-3 py-2.5 text-sm"
        >
          <span class="font-medium">{{ card.front }}</span>
          <div class="flex items-center gap-2">
            <FlashcardStatusBadge :status="card.status" />
            <button
              type="button"
              class="text-xs font-medium text-primary underline-offset-4 hover:underline"
            >
              Show answer
            </button>
          </div>
        </li>
      </ul>
      <p
        v-else
        class="rounded-lg border border-dashed border-border/80 px-3 py-6 text-center text-xs text-muted-foreground"
      >
        No cards yet — add one above.
      </p>
    </div>
  </Card>
</template>
