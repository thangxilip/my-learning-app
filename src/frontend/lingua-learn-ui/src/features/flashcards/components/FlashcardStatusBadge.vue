<script setup lang="ts">
import type { CardStatus } from '@/features/flashcards/types'
import { statusLabels, statusPresentation } from '@/features/flashcards/statusStyles'

const props = defineProps<{
  status: CardStatus
}>()
</script>

<template>
  <span
    data-slot="flashcard-status-badge"
    :class="[
      'inline-flex items-center rounded-full border px-2 py-0.5 text-xs font-medium',
      statusPresentation[props.status].badge,
    ]"
  >
    {{ statusLabels[props.status] }}
  </span>
</template>
