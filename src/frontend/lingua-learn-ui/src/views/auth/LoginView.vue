<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'

import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { ApiException, Client, LoginRequest } from '@/api/auth.generated'
import LoginForm from '@/features/auth/components/LoginForm.vue'
import type { LoginFormValues } from '@/features/auth/types'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

const submitting = ref(false)
const errorMessage = ref<string | null>(null)

function loginErrorMessage(err: unknown): string {
  if (ApiException.isApiException(err)) {
    return err.message
  }
  if (err && typeof err === 'object') {
    const detail = (err as { detail?: string }).detail
    const title = (err as { title?: string }).title
    if (typeof detail === 'string' && detail.trim()) return detail
    if (typeof title === 'string' && title.trim()) return title
  }
  if (err instanceof Error) return err.message
  return 'Could not sign in. Try again.'
}

async function handleLogin(values: LoginFormValues) {
  errorMessage.value = null
  submitting.value = true

  const baseUrl = import.meta.env.VITE_API_BASE_URL?.replace(/\/$/, '') ?? ''
  const client = new Client(baseUrl)

  try {
    const body = new LoginRequest({ email: values.email, password: values.password })
    const response = await client.login(body)
    auth.setSession(response, values.rememberMe)
    await router.push({ name: 'home' })
  } catch (err) {
    errorMessage.value = loginErrorMessage(err)
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <main class="min-h-screen bg-background text-foreground">
    <section class="grid min-h-screen lg:grid-cols-[1fr_30rem]">
      <div class="hidden bg-zinc-950 text-white lg:block">
        <div class="flex h-full flex-col justify-between px-12 py-10">
          <div class="text-lg font-semibold">Lingua Learn</div>
          <div class="max-w-lg space-y-5">
            <p class="text-4xl font-semibold leading-tight">Build a steadier language practice, one review at a time.</p>
            <p class="text-base leading-7 text-zinc-300">
              Keep focused sessions, review due cards, and track learning momentum from one quiet workspace.
            </p>
          </div>
          <p class="text-sm text-zinc-400">Adaptive flashcards for serious learners</p>
        </div>
      </div>

      <div class="flex min-h-screen items-center justify-center px-6 py-10">
        <Card class="w-full max-w-sm">
          <CardHeader>
            <CardDescription>Welcome back</CardDescription>
            <CardTitle class="text-3xl">Sign in to your account</CardTitle>
            <CardDescription>
              Continue to your learning dashboard.
            </CardDescription>
          </CardHeader>

          <CardContent class="space-y-4">
            <p
              v-if="errorMessage"
              class="rounded-md border border-destructive/40 bg-destructive/10 px-3 py-2 text-sm text-destructive"
              role="alert"
            >
              {{ errorMessage }}
            </p>
            <LoginForm :submitting="submitting" @submit="handleLogin" />
          </CardContent>

          <CardFooter class="justify-center">
            <p class="text-center text-sm text-muted-foreground">
              New to Lingua Learn?
              <a class="font-medium text-foreground underline-offset-4 hover:underline" href="/register">Create an account</a>
            </p>
          </CardFooter>
        </Card>
      </div>
    </section>
  </main>
</template>
