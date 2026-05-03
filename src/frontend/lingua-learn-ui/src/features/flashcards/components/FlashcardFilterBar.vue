<script setup lang="ts">
import { statusPresentation, statusLabels } from '@/features/flashcards/statusStyles'
import type { CardStatus, FlashcardFilter } from '@/features/flashcards/types'
import { cn } from '@/lib/utils'

const props = defineProps<{
  modelValue: FlashcardFilter
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: FlashcardFilter): void
}>()

const items: { value: FlashcardFilter; label: string; dot?: CardStatus }[] = [
  { value: 'all', label: 'All' },
  { value: 'new', label: statusLabels.new, dot: 'new' },
  { value: 'learning', label: statusLabels.learning, dot: 'learning' },
  { value: 'review', label: statusLabels.review, dot: 'review' },
  { value: 'mastered', label: statusLabels.mastered, dot: 'mastered' },
]

function select(value: FlashcardFilter) {
  emit('update:modelValue', value)
}
</script>

<template>
  <div
    class="flex flex-wrap items-center gap-2"
    role="group"
    aria-label="Filter cards by status"
  >
    <button
      v-for="item in items"
      :key="item.value"
      type="button"
      :class="
        cn(
          'inline-flex items-center gap-1.5 rounded-full border px-3 py-1.5 text-xs font-medium transition-colors',
          props.modelValue === item.value
            ? 'border-transparent bg-primary text-primary-foreground'
            : 'border-border bg-background text-foreground hover:bg-muted/80',
        )
      "
      @click="select(item.value)"
    >
      <span
        v-if="item.dot"
        class="size-1.5 shrink-0 rounded-full"
        :class="statusPresentation[item.dot].dot"
        aria-hidden="true"
      />
      {{ item.label }}
    </button>
  </div>
</template>
