<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'

import { Button } from '@/components/ui/button'
import MainWorkspace from '@/features/dashboard/components/MainWorkspace.vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()

const greeting = computed(() => 'Welcome back')

function signOut() {
  auth.clearSession()
  void router.push({ name: 'login' })
}
</script>

<template>
  <div class="min-h-screen bg-zinc-50 text-foreground dark:bg-background">
    <header
      class="border-b border-border/80 bg-card/80 backdrop-blur supports-[backdrop-filter]:bg-card/60"
    >
      <div
        class="mx-auto flex max-w-6xl items-center justify-between gap-4 px-4 py-3 sm:px-6"
      >
        <div class="min-w-0 space-y-0.5">
          <p class="text-xs font-medium uppercase tracking-wide text-muted-foreground">
            Lingua Learn
          </p>
          <p class="truncate text-sm font-medium">{{ greeting }}</p>
        </div>
        <Button variant="outline" size="sm" type="button" @click="signOut">
          Sign out
        </Button>
      </div>
    </header>

    <main class="mx-auto max-w-6xl px-4 py-8 sm:px-6 sm:py-10">
      <MainWorkspace />
    </main>
  </div>
</template>
